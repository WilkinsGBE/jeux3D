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

    [Header("Boss Health")]
    public GameObject BossHealthBarRoot;
    public Image BossHealthFill;
    public TMP_Text BossNameText;

    private BossHealth currentBoss;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        HideBossHealth();
    }

    void Update()
    {
        if (currentBoss == null)
            return;

        UpdateBossHealth();

        if (currentBoss.CurrentHealth <= 0)
            HideBossHealth();
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


    public void ShowBoss(BossHealth boss)
    {
        if (boss == null) return;

        currentBoss = boss;

        if (BossHealthBarRoot != null)
            BossHealthBarRoot.SetActive(true);

        if (BossNameText != null)
            BossNameText.text = boss.BossName;

        UpdateBossHealth();
    }

    public void HideBossHealth()
    {
        currentBoss = null;

        if (BossHealthBarRoot != null)
            BossHealthBarRoot.SetActive(false);
    }

    private void UpdateBossHealth()
    {
        if (BossHealthFill == null || currentBoss == null)
            return;

        float normalized = currentBoss.CurrentHealth / currentBoss.MaxHealth;
        BossHealthFill.fillAmount = normalized;
    }
}