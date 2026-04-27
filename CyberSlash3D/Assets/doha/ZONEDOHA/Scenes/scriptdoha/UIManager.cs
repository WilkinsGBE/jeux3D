using UnityEngine;
//doha
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    public Image HealthBar_Fill;
    public Image EnergyBar_Fill;

    public TMP_Text KeysText;
    public TMP_Text TerminalText;
    public TMP_Text EnergyText;

    public TMP_Text ScoreText;
    public TMP_Text TimerText;
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
        HealthBar_Fill.fillAmount = value;
    }

    public void SetEnergy(float value)
    {
        EnergyBar_Fill.fillAmount = value;
        EnergyText.text = Mathf.RoundToInt(value * 100) + "%";
    }


    public void SetScore(int value)
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + value;
    }


    public void SetKeys(int value)
    {
        KeysText.text = "Keys: " + value;
    }

    public void SetTerminals(int value)
    {
        TerminalText.text = "Terminals: " + value;
    }


    public void SetTimer(string value)
    {
        TimerText.text = value;
    }

    public void ShowHUD()
    {
        HUD.SetActive(true);
    }

    public void HideHUD()
    {
        HUD.SetActive(false);
    }
}