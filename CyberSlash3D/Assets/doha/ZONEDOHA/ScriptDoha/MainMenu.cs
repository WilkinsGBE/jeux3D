using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuPanel;
    public GameObject HowToPlayPanel;
    public GameObject GameHUD;


    void Start()
    {
        HideAllPanels();
        MainMenuPanel.SetActive(true);
        GameHUD.SetActive(false);

        Time.timeScale = 1f;
    }


    public void PlayGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Started");
    MainMenuPanel.SetActive(false);
    HowToPlayPanel.SetActive(false);
    GameHUD.SetActive(true);


    }

    public void OpenHowToPlay()
    {
        HideAllPanels();
        HowToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        HowToPlayPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }


    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    void HideAllPanels()
    {
        MainMenuPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}