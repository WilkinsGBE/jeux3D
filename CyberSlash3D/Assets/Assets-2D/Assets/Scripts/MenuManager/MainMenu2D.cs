using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("New Menu Panels")]
    public GameObject MenuPrincipale;
    public GameObject MenuSaisieSurnom;
    public GameObject MenuApropos;
    public GameObject Menu2D;
    public GameObject Menu3D;
    public GameObject Histoire2D;
    public GameObject Histoire3D;
    public GameObject InstructionMonde2D;
    public GameObject InstructionMonde3D;

    [Header("Pause Panel")]
    public GameObject PauseMenuPanel2D;

    private bool isPaused = false;

    [Header("Old Gameplay Panels")]
    public GameObject PlayerDeadPanel;
    public GameObject levelCompletePanel;

    [Header("Score Texts")]
    public TMP_Text resultBaseScoreText;
    public TMP_Text resultTimeText;
    public TMP_Text resultBonusText;
    public TMP_Text resultFinalScoreText;


    [Header("Nickname")]
    public TMP_InputField inputSurnom;
    public TMP_Text[] playerNameTexts;

    [Header("Buttons")]
    public Button continuerButton;

    [Header("Menu Music")]
    public AudioSource menuMusicSource;

    private const string PlayerNameKey = "PlayerName";

    private enum SelectedMode
    {
        None,
        Mode2D,
        Mode3D
    }

    private SelectedMode selectedMode = SelectedMode.None;

    void Start()
    {
        PlayerPrefs.DeleteKey(PlayerNameKey);

        HideAllPanels();
        MenuPrincipale.SetActive(true);
        Time.timeScale = 1f;

        if (menuMusicSource != null && !menuMusicSource.isPlaying)
            menuMusicSource.Play();

        LoadPlayerName();
        ValidateNicknameInput();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void StopMenuMusic()
    {
        if (menuMusicSource != null && menuMusicSource.isPlaying)
            menuMusicSource.Stop();
    }

    public void OpenPanel(GameObject panel)
    {
        HideAllPanels();

        if (panel != null)
            panel.SetActive(true);
    }

    public void OpenMenuPrincipale() => OpenPanel(MenuPrincipale);
    public void OpenMenuSaisieSurnom() => OpenPanel(MenuSaisieSurnom);
    public void OpenMenuApropos() => OpenPanel(MenuApropos);
    public void OpenMenu2D()
    {
        selectedMode = SelectedMode.Mode2D;
        OpenPanel(MenuSaisieSurnom);
    }
    public void OpenMenu3D()
    {
        selectedMode = SelectedMode.Mode3D;
        OpenPanel(MenuSaisieSurnom);
    }
    public void OpenHistoire2D() => OpenPanel(Histoire2D);
    public void OpenHistoire3D() => OpenPanel(Histoire3D);
    public void OpenInstructionMonde2D() => OpenPanel(InstructionMonde2D);
    public void OpenInstructionMonde3D() => OpenPanel(InstructionMonde3D);

    public void SaveNickname()
    {
        string playerName = inputSurnom.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            playerName = "Votre Surnom...";

        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.Save();

        UpdatePlayerNameTexts(playerName);

        if (selectedMode == SelectedMode.Mode2D)
        {
            OpenPanel(InstructionMonde2D);
        }
        else if (selectedMode == SelectedMode.Mode3D)
        {
            OpenPanel(InstructionMonde3D);
        }
        else
        {
            OpenPanel(MenuPrincipale);
        }
    }

    public void ValidateNicknameInput()
    {
        string text = inputSurnom.text.Trim();

        bool isValid =
            !string.IsNullOrEmpty(text) &&
            text != "Votre Surnom..." && text != "Joueur";

        continuerButton.interactable = isValid;
    }

    private void LoadPlayerName()
    {
        string savedName = PlayerPrefs.GetString(PlayerNameKey, "Votre Surnom...");

        if (inputSurnom != null)
            inputSurnom.text = savedName;

        UpdatePlayerNameTexts(savedName);
    }

    private void UpdatePlayerNameTexts(string playerName)
    {
        foreach (TMP_Text text in playerNameTexts)
        {
            if (text != null)
                text.text = playerName;
        }
    }

    public void PlayGame2D()
    {
        SceneManager.LoadScene("DohaScene1");
        HideAllPanels();
        Time.timeScale = 1f;
        Debug.Log("Jeu 2D lancé");
    }

    public void PlayGame3D()
    {
        SceneManager.LoadScene("Your3DSceneName");
        HideAllPanels();
        Time.timeScale = 1f;
        Debug.Log("Jeu 3D lancé");
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("StartInGameplay", 0);
        PlayerPrefs.Save();

        if (PersistentUI2D.instance != null)
        {
            Destroy(PersistentUI2D.instance.gameObject);
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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

    public void RestartFromPause()
    {
        isPaused = false;

        if (PauseMenuPanel2D != null)
            PauseMenuPanel2D.SetActive(false);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Debug.Log("Next level clicked");

        PersistentUI2D.instance.HideDeathPanel();
        PersistentUI2D.instance.HideLevelCompletePanel();

        HideScoreDetails();

        Time.timeScale = 1f;

        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 3)
        {
            StopMenuMusic();

            if (PersistentUI2D.instance != null)
                Destroy(PersistentUI2D.instance.gameObject);

            Play3DLevel1();
        }
        else if (currentScene == 4)
        {
            OpenMainMenu();
        }
        else
        {
            SceneManager.LoadScene(currentScene + 1);
        }
    }

    public void Play2DLevel1()
    {
        PlayerPrefs.DeleteKey("Total2DScore");
        PlayerPrefs.Save();

        SceneManager.LoadScene("WilkinsScene 1"); 
        HideAllPanels();
        Time.timeScale = 1f;
    }

    public void Play2DLevel2()
    {
        SceneManager.LoadScene("JohnScene");
        HideAllPanels();
        Time.timeScale = 1f;
    }

    public void Play2DLevel3()
    {
        SceneManager.LoadScene("DohaScene1");
        HideAllPanels();
        Time.timeScale = 1f;
    }

    public void Play3DLevel1()
    {
        StopMenuMusic();

        PlayerPrefs.SetInt("StartInGameplay", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("SampleScene");
    }

    public void Play3DLevel2()
    {
        StopMenuMusic();

        PlayerPrefs.SetInt("StartInGameplay", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("SampleScene");
    }

    public void Play3DLevel3()
    {
        StopMenuMusic();

        PlayerPrefs.SetInt("StartInGameplay", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("SampleScene");
    }

    public void ContinueFromInstruction2D()
    {
        OpenPanel(Menu2D);
    }

    public void ContinueFromInstruction3D()
    {
        OpenPanel(Menu3D);
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame called");

        isPaused = true;

        HideAllPanels();

        if (PauseMenuPanel2D != null)
        {
            PauseMenuPanel2D.SetActive(true);
            Debug.Log("Pause panel activated");
        }
        else
        {
            Debug.LogError("PauseMenuPanel2D is NOT assigned!");
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (PauseMenuPanel2D != null)
            PauseMenuPanel2D.SetActive(false);

        Time.timeScale = 1f;
    }

    public void HideAllPanels()
    {
        if (MenuPrincipale != null)
            MenuPrincipale.SetActive(false);

        if (MenuSaisieSurnom != null)
            MenuSaisieSurnom.SetActive(false);

        if (MenuApropos != null)
            MenuApropos.SetActive(false);

        if (Menu2D != null)
            Menu2D.SetActive(false);

        if (Menu3D != null)
            Menu3D.SetActive(false);

        if (Histoire2D != null)
            Histoire2D.SetActive(false);

        if (Histoire3D != null)
            Histoire3D.SetActive(false);

        if (InstructionMonde2D != null)
            InstructionMonde2D.SetActive(false);

        if (InstructionMonde3D != null)
            InstructionMonde3D.SetActive(false);

        if (PlayerDeadPanel != null)
            PlayerDeadPanel.SetActive(false);

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);

        if (PauseMenuPanel2D != null)
            PauseMenuPanel2D.SetActive(false);
    }

    public void HidePlayerDeadPanel()
    {
        if (PlayerDeadPanel != null)
            PlayerDeadPanel.SetActive(false);
    }

    public void HideScoreDetails()
    {
        resultBaseScoreText.gameObject.SetActive(false);
        resultTimeText.gameObject.SetActive(false);
        resultBonusText.gameObject.SetActive(false);
        resultFinalScoreText.gameObject.SetActive(false);
    }

    public void ShowScoreDetails()
    {
        resultBaseScoreText.gameObject.SetActive(true);
        resultTimeText.gameObject.SetActive(true);
        resultBonusText.gameObject.SetActive(true);
        resultFinalScoreText.gameObject.SetActive(true);
    }
}