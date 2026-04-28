using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Health")]
    public Image HealthBar_Fill;

    [Header("Stats")]
    public TMP_Text KeysText;
    public TMP_Text TerminalText;
    public TMP_Text ScoreText;
    public TMP_Text TimerText;

    [Header("HUD")]
    public GameObject HUD;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetHealth(float value)
    {
        if (HealthBar_Fill != null)
            HealthBar_Fill.fillAmount = value;
    }

    public void SetScore(int value)
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + value;
    }

    public void SetKeys(int value)
    {
        if (KeysText != null)
            KeysText.text = "Keys: " + value;
    }

    public void SetTerminals(int value)
    {
        if (TerminalText != null)
            TerminalText.text = "Terminals: " + value;
    }

    public void SetTimer(string value)
    {
        if (TimerText != null)
            TimerText.text = value;
    }

    public void ShowHUD()
    {
        if (HUD != null)
            HUD.SetActive(true);
    }

    public void HideHUD()
    {
        if (HUD != null)
            HUD.SetActive(false);
    }
}