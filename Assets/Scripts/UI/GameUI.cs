using System.Collections;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // === HEALTH BAR ===
    [SerializeField] private GameObject[] healthBars;

    // === WAVE TEXT ===
    [SerializeField] private TextMeshProUGUI waveText;

    // === NEXT WAVE TEXT ===
    [SerializeField] private TextMeshProUGUI nextWaveText;

    [SerializeField] private CanvasGroup nextWaveGroup;

    // === REFERENCE TO SPAWN MANAGER ===
    [SerializeField] private SpawnManager spawnManager;

    // === TEXT FADING ===
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private int countdownTime = 3;

    // === ENEMIES REMAINING TEXT ===
    [SerializeField] private TextMeshProUGUI enemiesRemainingText;

    private int totalEnemies;

    // Update the health ui based on current player health
    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].SetActive(i < currentHealth);
        }
    }

    // Update the wave text on top of the screen
    public void UpdateWave(int currentWave)
    {
        waveText.text = "Wave: " + currentWave.ToString();
    }

    // Fade in and Fade out next wave text
    public IEnumerator ShowNextWave()
    {
        nextWaveGroup.alpha = 0f;
        nextWaveGroup.gameObject.SetActive(true);

        int currentWave = spawnManager.currentWave + 1;

        nextWaveText.text = $"Wave {currentWave} Complete! Next in 3...";

        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            nextWaveGroup.alpha = t / fadeDuration;
            yield return null;
        }
        nextWaveGroup.alpha = 1f;

        // Countdown
        for (int i = countdownTime; i >= 1; i--)
        {
            nextWaveText.text = $"Wave {currentWave} Complete! Next in {i}...";
            yield return new WaitForSeconds(1f);
        }

        // Fade Out

        for (float t = fadeDuration; t > 0; t -= Time.deltaTime)
        {
            nextWaveGroup.alpha = t / fadeDuration;
            yield return null;
        }
        nextWaveGroup.alpha = 0f;
        nextWaveGroup.gameObject.SetActive(false);
    }

    // Fade in and fade out first wave text
    public IEnumerator ShowFirstWaveText()
    {
        nextWaveGroup.alpha = 0f;
        nextWaveGroup.gameObject.SetActive(true);

        nextWaveText.text = $"First wave spawning in 3...";

        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            nextWaveGroup.alpha = t / fadeDuration;
            yield return null;
        }
        nextWaveGroup.alpha = 1f;

        // Countdown
        for (int i = countdownTime; i >= 1; i--)
        {
            nextWaveText.text = $"First wave spawning in {i}...";
            yield return new WaitForSeconds(1f);
        }

        // Fade Out

        for (float t = fadeDuration; t > 0; t -= Time.deltaTime)
        {
            nextWaveGroup.alpha = t / fadeDuration;
            yield return null;
        }
        nextWaveGroup.alpha = 0f;
        nextWaveGroup.gameObject.SetActive(false);
    }

    public void SetTotalEnemyCountText(int total)
    {
        // Get number of total enemies from spawn manager
        totalEnemies = total;
    }

    // Update the text that shows enemycount on screen
    public void UpdateEnemyCountText(int currentEnemies)
    {
        // Update text
        enemiesRemainingText.text = $"Enemies: {currentEnemies}/{totalEnemies}";
    }
}