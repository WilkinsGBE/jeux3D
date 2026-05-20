using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject HowToPlayPanel;
    public GameObject GameHUD;
    public GameObject PauseMenuPanel;

    public GameObject MenuPrincipalPanel;

    [Header("Menu Music")]
    public AudioSource menuMusicSource;

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
        PlayMenuMusic();

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
        StopMenuMusic();

        // ✅ START AMBIENT MUSIC HERE
        if (GameManager.instance != null)
            GameManager.instance.PlayAmbientMusic();

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
        // ❌ DO NOT play music here (you wanted no music during pause)
        if (GameManager.instance != null)
            GameManager.instance.StopAmbientMusic();

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
        // ✅ Resume ambient music
        if (GameManager.instance != null)
            GameManager.instance.PlayAmbientMusic();

        HideAllPanels();

        isPaused = false;

        Time.timeScale = 1f;

        StartCoroutine(LockCursorNextFrame());
    }

    public void RestartGame()
    {
        StopMenuMusic();

        if (GameManager.instance != null)
            GameManager.instance.StopAmbientMusic();

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

        PlayerPrefs.SetInt("StartInGameplay", 0);
        PlayerPrefs.Save();

        if (GameManager.instance != null)
            GameManager.instance.StopAmbientMusic();

        if (PersistentUI2D.instance != null)
            Destroy(PersistentUI2D.instance.gameObject);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private IEnumerator LockCursorNextFrame()
    {
        yield return null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenHowToPlay()
    {
        PlayMenuMusic();

        HideAllPanels();

        if (HowToPlayPanel != null)
            HowToPlayPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseHowToPlay()
    {
        PlayMenuMusic();

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

    private void PlayMenuMusic()
    {
        if (menuMusicSource != null && !menuMusicSource.isPlaying)
            menuMusicSource.Play();
    }

    private void StopMenuMusic()
    {
        if (menuMusicSource != null && menuMusicSource.isPlaying)
            menuMusicSource.Stop();
    }

    public void QuitGame()
    {
        StopMenuMusic();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}