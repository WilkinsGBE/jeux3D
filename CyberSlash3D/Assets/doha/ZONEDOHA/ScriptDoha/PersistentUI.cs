using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI instance;

    public GameObject DeathMenuPanel;
    public GameObject VictoryMenuPanel;

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
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        HideAll();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAll();
    }

    public void ShowDeath()
    {
        HideAll();

        if (DeathMenuPanel != null)
            DeathMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        HideAll();

        if (VictoryMenuPanel != null)
            VictoryMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void HideAll()
    {
        if (DeathMenuPanel != null)
            DeathMenuPanel.SetActive(false);

        if (VictoryMenuPanel != null)
            VictoryMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}