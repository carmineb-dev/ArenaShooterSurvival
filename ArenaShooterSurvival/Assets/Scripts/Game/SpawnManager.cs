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

    // // === REFERENCE TO PLAYER TRANSFORM COMPONENT ===
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        // Calculate the arena bounds in which spawning is allowed
        minX = arenaBounds.bounds.min.x;
        minY = arenaBounds.bounds.min.y;
        maxX = arenaBounds.bounds.max.x;
        maxY = arenaBounds.bounds.max.y;

        // Spawn the first wave
        StartWave(currentWave);
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

        // Start to spawn enemies
        SpawnEnemies(chasers, fast, tanks);
    }

    // Get a spawn position that don't collide with other objects
    private Vector3 GetValidSpawnPoint()
    {
        Vector3 spawnPos;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            // Generate random spawn position
            float spawnPosX = Random.Range(minX, maxX);
            float spawnPosY = Random.Range(minY, maxY);
            spawnPos = new Vector3(spawnPosX, spawnPosY, 0);
            attempts++;

            // Check if spawnPos is free
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.5f);

            // Free position
            if (hit == null)
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

        // Pass player reference
        enemyScript.Initialize(playerTransform);
    }

    // Everytime an enemy is destroyed
    public void EnemyDestroyed()
    {
        // Decrement enemy count
        enemyCount--;

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
        // Shows 3 seconds countdown text
        yield return StartCoroutine(gameUI.ShowNextWave());

        // Increment the current wave index
        currentWave++;

        // Spawn the new wave
        StartWave(currentWave);
    }
}