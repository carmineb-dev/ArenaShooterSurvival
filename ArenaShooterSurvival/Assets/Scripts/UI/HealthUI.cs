using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject[] healthBars;

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].SetActive(i < currentHealth);
        }
    }
}