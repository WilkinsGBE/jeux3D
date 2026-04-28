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

    IEnumerator Start()
    {
        yield return null;
        ShowMainMenu();
    }

    void Update()
    {
        if (!gameStarted)
            return;

        if (Input.GetKeyDown(KeyCode.P))
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
        Debug.Log("Game Started");

        HideAllPanels();

        gameStarted = true;
        isPaused = false;

        if (GameHUD != null)
            GameHUD.SetActive(true);

        Time.timeScale = 1f;

        StartCoroutine(LockCursorNextFrame());
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

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}