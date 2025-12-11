using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // === WAVE SURVIVED ===
    [SerializeField] private TextMeshProUGUI waveText;

    // === HIGH SCORE ===
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private ScoreManager scoreManager;

    // Setup of the scene
    public void Setup(int waveSurvived)
    {
        gameObject.SetActive(true);
        waveText.text = "Waves survived: " + waveSurvived.ToString();
        ShowHighScore(waveSurvived);
    }

    // Restart of the game
    public void RestartButton()
    {
        SceneManager.LoadScene(1);
    }

    // Exit from the game
    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }

    // Show and eventually update high score text
    public void ShowHighScore(int finalScore)
    {
        if (finalScore > scoreManager.GetHighScore())
        {
            scoreManager.CheckNewScore(finalScore);
            highScoreText.text = $"High Score: {scoreManager.GetHighScore()} (NEW!)";
        }
        else
        {
            highScoreText.text = $"High Score: {scoreManager.GetHighScore()}";
        }
    }
}