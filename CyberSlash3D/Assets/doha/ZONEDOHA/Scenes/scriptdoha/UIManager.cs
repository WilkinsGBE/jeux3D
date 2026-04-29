using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton pour accéder facilement au UIManager depuis n’importe quel script
    public static UIManager instance;

    [Header("Health")]
    public Image HealthBar_Fill; // barre de vie (fill image)

    [Header("Stats")]
    public TMP_Text KeysText;      // affichage des clés
    public TMP_Text TerminalText;  // affichage des terminaux activés
    public TMP_Text ScoreText;     // score joueur
    public TMP_Text TimerText;     // timer du jeu

    [Header("HUD")]
    public GameObject HUD; // panel principal HUD

    void Awake()
    {
        // gestion du singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // ===================== HEALTH =====================
    public void SetHealth(float value)
    {
        // update barre de vie (0 → 1)
        if (HealthBar_Fill != null)
            HealthBar_Fill.fillAmount = value;
    }

    // ===================== SCORE =====================
    public void SetScore(int value)
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + value;
    }

    // ===================== KEYS =====================
    public void SetKeys(int value)
    {
        if (KeysText != null)
            KeysText.text = "Keys: " + value;
    }

    // ===================== TERMINALS =====================
    public void SetTerminals(int value)
    {
        if (TerminalText != null)
            TerminalText.text = "Terminals: " + value;
    }

    // ===================== TIMER =====================
    public void SetTimer(string value)
    {
        if (TimerText != null)
            TimerText.text = value;
    }

    // ===================== HUD CONTROL =====================
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