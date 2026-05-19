using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class MainMenu2D : MonoBehaviour
{
    public GameObject HowToPlayPanel;
    public GameObject MainMenuPanel;
    public GameObject PlayerDeadPanel;
    public GameObject levelCompletePanel;
    public TMP_Text resultBaseScoreText;
    public TMP_Text resultTimeText;
    public TMP_Text resultBonusText;
    public TMP_Text resultFinalScoreText;

    void Start()
    {
        HideAllPanels();

        MainMenuPanel.SetActive(true);
        Time.timeScale = 1f;
    }
    public void PlayGame()
    {
        //Test
        SceneManager.LoadScene("DohaScene1");
        //SceneManager.LoadScene("WilkinsScene 1");
        HideAllPanels();
        Time.timeScale = 1f;
        Debug.Log("Jeu lance");
    }

    public void OpenHowToPlay()
    {
        HowToPlayPanel.SetActive(true);
    }

    public void OpenMainMenu()
    {
        PersistentUI2D.instance.HideDeathPanel();
        PersistentUI2D.instance.HideLevelCompletePanel();
        SceneManager.LoadScene(0);
    }

    public void CloseHowToPlay()
    {
        HowToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void Revivre()
    {
        Debug.Log("Restart clicked");
        PersistentUI2D.instance.HideDeathPanel();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Debug.Log("Next level click�");

        PersistentUI2D.instance.HideDeathPanel();
        PersistentUI2D.instance.HideLevelCompletePanel();
        HideScoreDetails();

        Time.timeScale = 1f;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 3)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    public void HideAllPanels()
    {
        MainMenuPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);
        PlayerDeadPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
    }

    public void HidePlayerDeadPanel()
    {
        if (PlayerDeadPanel != null)
            PlayerDeadPanel.SetActive(false);
    }

    public void HideScoreDetails()
    {
        resultBaseScoreText.gameObject.SetActive(false);
        resultTimeText.gameObject.SetActive(false); ;
        resultBonusText.gameObject.SetActive(false); ;
        resultFinalScoreText.gameObject.SetActive(false); ;
    }

    public void ShowScoreDetails()
    {
        resultBaseScoreText.gameObject.SetActive(true);
        resultTimeText.gameObject.SetActive(true); ;
        resultBonusText.gameObject.SetActive(true); ;
        resultFinalScoreText.gameObject.SetActive(true); ;
    }
}