using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUI2D : MonoBehaviour
{
    public static PersistentUI2D instance;
    public GameObject playerDeadPanel;
    public GameObject LevelCompletePanel;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Prévient doublons
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideDeathPanel();
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
}