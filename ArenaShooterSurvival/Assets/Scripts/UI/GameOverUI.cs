using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    public void Setup(int waveSurvived)
    {
        gameObject.SetActive(true);
        waveText.text = "Waves survived: " + waveSurvived.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}