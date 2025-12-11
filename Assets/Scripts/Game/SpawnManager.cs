using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // === WAVES ===
    public int currentWave = 0;

    private int displayWave;

    // === ENEMY ===
    [SerializeField] private GameObject enemyChaserPrefab;

    [SerializeField] private GameObject enemyFastPrefab;
    [SerializeField] private GameObject enemyTankPrefab;

    private int enemyCount;

    // === ARENA BOUNDS ===
    [SerializeField] private BoxCollider2D arenaBounds;

    private float minX, maxX, minY, maxY;

    // === REFERENCE TO GAMEUI ===
    [SerializeField] private GameUI gameUI;

    // === REFERENCE TO PLAYER TRANSFORM COMPONENT ===
    [SerializeField] private Transform playerTransform;

    // === AUDIO CLIP ===
    [SerializeField] private AudioClip waveStart;

    [SerializeField] private AudioClip winSound;

    // === SPAWN ===
    [SerializeField] private int maxAttempts = 30;

    [SerializeField] private float spawnCheckRadius = 0.5f;

    [SerializeField] private float minDistanceFromPlayer = 7f;

    private void Start()
    {
        // Calculate the arena bounds in which spawning is allowed
        minX = arenaBounds.bounds.min.x;
        minY = arenaBounds.bounds.min.y;
        maxX = arenaBounds.bounds.max.x;
        maxY = arenaBounds.bounds.max.y;

        StartCoroutine(StartFirstWave());
    }

    // === Spawn a new wave ===
    private void StartWave(int currentWave)
    {
        int chasers, fast, tanks;

        // Refresh UI Wave text
        displayWave = currentWave + 1;
        gameUI.UpdateWave(displayWave);

        // Set amount of each type of enemy
        if (currentWave <= 3)
        {
            chasers = 3 + currentWave;
            fast = 0;
            tanks = 0;
        }
        else if (currentWave <= 7)
        {
            chasers = 3 + (currentWave / 2);
            fast = currentWave - 3;
            tanks = 0;
        }
        else
        {
            chasers = 4;
            fast = 3 + (currentWave - 7);
            tanks = 1 + (currentWave - 8) / 2;
        }

        // Set enemy count
        enemyCount = chasers + fast + tanks;

        // Set Total in enemy count text
        gameUI.SetTotalEnemyCountText(enemyCount);
        // Update enemy count text
        gameUI.UpdateEnemyCountText(enemyCount);

        // Start to spawn enemies
        SpawnEnemies(chasers, fast, tanks);
    }

    // Get a spawn position that don't collide with other objects
    private Vector3 GetValidSpawnPoint()
    {
        Vector3 spawnPos;
        int attempts = 0;

        do
        {
            // Generate random spawn position
            float spawnPosX = Random.Range(minX, maxX);
            float spawnPosY = Random.Range(minY, maxY);
            spawnPos = new Vector3(spawnPosX, spawnPosY, 0);
            attempts++;

            // Check if spawnPos is free from obstacles
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, spawnCheckRadius);

            // Check distance from player
            float distanceToPlayer = Vector3.Distance(spawnPos, playerTransform.position);
            bool isFarEnoughFromPlayer = distanceToPlayer >= minDistanceFromPlayer;

            // Free position and far from player
            if (hit == null && isFarEnoughFromPlayer)
            {
                return spawnPos;
            }
        } while (attempts < maxAttempts);

        // Spawn anyway after 10 attempts
        return spawnPos;
    }

    // Spawn the enemies
    private void SpawnEnemies(int chasers, int fast, int tanks)
    {
        for (int i = 0; i < chasers; i++)
        {
            Spawn(enemyChaserPrefab);
        }

        for (int i = 0; i < fast; i++)
        {
            Spawn(enemyFastPrefab);
        }

        for (int i = 0; i < tanks; i++)
        {
            Spawn(enemyTankPrefab);
        }
    }

    private void Spawn(GameObject enemyToSpawn)
    {
        // Get a valid spawn point
        Vector3 spawnPos = GetValidSpawnPoint();

        // Instantiate a new enemy at the spawn position with no rotation
        GameObject enemyObj = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

        // Get the Enemy script from the spawned enemy
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();

        // Fade in animation
        enemyScript.SpawnAnimation();

        // Pass player reference
        enemyScript.Initialize(playerTransform);
    }

    // Everytime an enemy is destroyed
    public void EnemyDestroyed()
    {
        // Decrement enemy count
        enemyCount--;

        // Update enemy count text
        gameUI.UpdateEnemyCountText(enemyCount);

        // Check if there are no other enemies
        if (enemyCount <= 0)
        {
            // Start the countdown and spawn a new wave
            StartCoroutine(StartNextWaveCountdown());
        }
    }

    // Countdown to the next wave
    private IEnumerator StartNextWaveCountdown()
    {
        // Play win sfx
        AudioManager.instance.playSfx(winSound, 0.8f);

        // Shows 3 seconds countdown text
        yield return StartCoroutine(gameUI.ShowNextWave());

        // Play the start wave sfx
        AudioManager.instance.playSfx(waveStart, 1f);

        // Increment the current wave index
        currentWave++;

        // Spawn the new wave
        StartWave(currentWave);
    }

    private IEnumerator StartFirstWave()
    {
        // Shows 3 seconds countdown text
        yield return StartCoroutine(gameUI.ShowFirstWaveText());

        // Play the start wave sfx
        AudioManager.instance.playSfx(waveStart, 1f);

        // Spawn the first wave
        StartWave(currentWave);
    }

    // Get enemy count
    public int CurrentEnemyCount
    {
        get { return enemyCount; }
    }
}