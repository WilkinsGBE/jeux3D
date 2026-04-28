using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health UI")]
    public Image healthFill;

    [Header("Text UI")]
    public TMP_Text scoreText;
    public TMP_Text timerText;

    private PlayerHealth playerHealth;

    public void Setup(PlayerHealth health)
    {
        playerHealth = health;
        UpdateHealthUI();
    }

    private void Update()
    {
        if (playerHealth != null)
            UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthFill == null || playerHealth == null) return;

        healthFill.fillAmount =
            (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void SetTimer(string time)
    {
        if (timerText != null)
            timerText.text = time;
    }
}