using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int currentWave = 1;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameOverUI gameOverScreen;
    [SerializeField] private CursorManager cursor;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (player.isGameOver)
        {
            Time.timeScale = 0;
            cursor.SetDefaultCursor();
            gameOverScreen.Setup(currentWave);
        }
    }
}