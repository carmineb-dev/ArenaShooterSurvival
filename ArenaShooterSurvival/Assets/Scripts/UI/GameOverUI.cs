using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    // Setup of the scene
    public void Setup(int waveSurvived)
    {
        gameObject.SetActive(true);
        waveText.text = "Waves survived: " + waveSurvived.ToString();
    }

    // Restart of the game
    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}