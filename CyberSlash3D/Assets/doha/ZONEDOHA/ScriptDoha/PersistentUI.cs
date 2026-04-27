using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUI : MonoBehaviour
{
     public static PersistentUI instance;

    public GameObject PauseMenuPanel;
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
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAll();
    }


    public void ShowPause()
    {
        HideAll();
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    
    public void ShowDeath()
    {
        HideAll();
        DeathMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

   
    public void ShowVictory()
    {
        HideAll();
        VictoryMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }


    public void HideAll()
    {
        if (PauseMenuPanel) PauseMenuPanel.SetActive(false);
        if (DeathMenuPanel) DeathMenuPanel.SetActive(false);
        if (VictoryMenuPanel) VictoryMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) ShowPause();
        if (Input.GetKeyDown(KeyCode.O)) ShowDeath();
        if (Input.GetKeyDown(KeyCode.I)) ShowVictory();
        if (Input.GetKeyDown(KeyCode.R)) HideAll();
    }
}