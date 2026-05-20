using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PersistentUI2D : MonoBehaviour
{
    public static PersistentUI2D instance;
    public GameObject playerDeadPanel;
    public GameObject LevelCompletePanel;

    [Header("Player Name")]
    public TMP_Text playerNameText;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            UpdatePlayerName();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideDeathPanel();
        HideLevelCompletePanel();

        UpdatePlayerName();

        if (playerNameText != null)
        {
            bool isMenu = scene.name == "MainMenu";

            playerNameText.gameObject.SetActive(!isMenu);
        }
    }

    public void ShowDeathPanel()
    {
        if (playerDeadPanel != null)
            playerDeadPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void HideDeathPanel()
    {
        if (playerDeadPanel != null)
            playerDeadPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void HideLevelCompletePanel()
    {
        if (LevelCompletePanel != null)
            LevelCompletePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void UpdatePlayerName()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Joueur");

        if (playerNameText != null)
            playerNameText.text = playerName;
    }
}