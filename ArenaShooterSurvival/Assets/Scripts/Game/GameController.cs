using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int currentWave;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameOverUI gameOverScreen;
    [SerializeField] private CursorManager cursor;
    [SerializeField] private SpawnManager spawnManager;

    private void Start()
    {
        // Set timeScale to 1
        Time.timeScale = 1;
    }

    private void Update()
    {
        // If player dies stops the game, change cursor and shows the game over screen with the current wave index
        if (player.isGameOver)
        {
            Time.timeScale = 0;
            cursor.SetDefaultCursor();
            gameOverScreen.Setup(spawnManager.currentWave);
        }
    }
}