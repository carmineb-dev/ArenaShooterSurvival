using Unity.Hierarchy;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int highScore;

    private void Start()
    {
        // Read saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Check if new score is higher than highScore
    public void CheckNewScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }
}