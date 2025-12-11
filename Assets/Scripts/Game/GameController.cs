using UnityEngine;

public class GameController : MonoBehaviour
{
    // === REFERENCE TO PLAYER ===
    [SerializeField] private PlayerController player;

    // === REFERENCE TO GAMEOVER UI ===
    [SerializeField] private GameOverUI gameOverScreen;

    // === REFERENCE TO CURSOR MANAGER ===
    [SerializeField] private CursorManager cursor;

    // === REFERENCE TO SPAWN MANAGER ===
    [SerializeField] private SpawnManager spawnManager;

    // === GAME OVER ===
    private bool hasGameEnded = false;

    private void Start()
    {
        // Set timeScale to 1
        Time.timeScale = 1;
    }

    private void Update()
    {
        // If player dies stops the game, change cursor and shows the game over screen with the current wave index
        if (player.isGameOver && !hasGameEnded)
        {
            hasGameEnded = true;
            HandleGameOver();
        }
    }

    // Manage the game when is game over
    private void HandleGameOver()
    {
        Time.timeScale = 0;
        cursor.SetDefaultCursor();
        gameOverScreen.Setup(spawnManager.currentWave);
    }
}