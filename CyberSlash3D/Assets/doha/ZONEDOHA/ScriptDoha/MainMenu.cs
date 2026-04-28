using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject HowToPlayPanel;
    public GameObject GameHUD;
    public GameObject PauseMenuPanel;

    private bool gameStarted = false;
    private bool isPaused = false;

    private const string StartInGameplayKey = "StartInGameplay";

    IEnumerator Start()
    {
        yield return null;

        if (PlayerPrefs.GetInt(StartInGameplayKey, 0) == 1)
        {
            PlayerPrefs.SetInt(StartInGameplayKey, 0);
            PlayerPrefs.Save();

            PlayGame();
        }
        else
        {
            ShowMainMenu();
        }
    }

    void Update()
    {
        if (!gameStarted)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ShowMainMenu()
    {
        HideAllPanels();

        gameStarted = false;
        isPaused = false;

        if (MainMenuPanel != null)
            MainMenuPanel.SetActive(true);

        if (GameHUD != null)
            GameHUD.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        HideAllPanels();

        gameStarted = true;
        isPaused = false;

        if (GameHUD != null)
            GameHUD.SetActive(true);

        Time.timeScale = 1f;

        StartCoroutine(StartGameplayNextFrame());
    }

    public void PauseGame()
    {
        HideAllPanels();

        isPaused = true;

        if (PauseMenuPanel != null)
            PauseMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        HideAllPanels();

        isPaused = false;

        Time.timeScale = 1f;

        StartCoroutine(LockCursorNextFrame());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt(StartInGameplayKey, 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Respawn()
    {
        RestartGame();
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt(StartInGameplayKey, 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LockCursorNextFrame()
    {
        yield return null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenHowToPlay()
    {
        HideAllPanels();

        if (HowToPlayPanel != null)
            HowToPlayPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseHowToPlay()
    {
        HideAllPanels();

        if (MainMenuPanel != null)
            MainMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideAllPanels()
    {
        if (MainMenuPanel != null)
            MainMenuPanel.SetActive(false);

        if (HowToPlayPanel != null)
            HowToPlayPanel.SetActive(false);

        if (PauseMenuPanel != null)
            PauseMenuPanel.SetActive(false);
    }

    private IEnumerator StartGameplayNextFrame()
    {
        yield return null;

        HideAllPanels();

        if (GameHUD != null)
            GameHUD.SetActive(true);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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