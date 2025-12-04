using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int currentWave = 0;
    private int displayWave;

    [SerializeField] private GameObject enemyPrefab;

    private int enemyCount;

    // === ARENA BOUNDS ===
    [SerializeField] private BoxCollider2D arenaBounds;

    private float minX, maxX, minY, maxY;

    // === REFERENCE TO GAMEUI ===
    [SerializeField] private GameUI gameUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        minX = arenaBounds.bounds.min.x;
        minY = arenaBounds.bounds.min.y;
        maxX = arenaBounds.bounds.max.x;
        maxY = arenaBounds.bounds.max.y;

        StartWave(currentWave);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void StartWave(int currentWave)
    {
        // Refresh UI Wave text
        displayWave = currentWave + 1;
        gameUI.UpdateWave(displayWave);

        // Calculate enemy to spawn each wave
        int enemyToSpawn = 3 + (currentWave * 2);

        // Spawn enemies at random position
        for (int i = 0; i < enemyToSpawn; i++)
        {
            // Generate random spawn position
            float spawnPosX = Random.Range(minX, maxX);
            float spawnPosY = Random.Range(minY, maxY);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0);

            // Instantiate a new enemy at the spawn position with no rotation
            GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // Get the EnemyChaser script from the spawned enemy
            EnemyChaser enemyScript = enemyObj.GetComponent<EnemyChaser>();

            // Assign this SpawnManager to the enemy so it can notify when it dies
            enemyScript.SetSpawnManager(this);

            // Increment enemy count
            enemyCount++;
        }
    }

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

    private IEnumerator StartNextWaveCountdown()
    {
        // Shows 3 seconds countdown
        yield return StartCoroutine(gameUI.ShowNextWave());

        // Increment the current wave index
        currentWave++;

        // Spawn the new wave
        StartWave(currentWave);
    }
}