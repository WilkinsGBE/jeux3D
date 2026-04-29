using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUI : MonoBehaviour
{
    // Singleton (une seule instance qui reste entre les scènes)
    public static PersistentUI instance;

    [Header("UI Panels")]
    public GameObject DeathMenuPanel;
    public GameObject VictoryMenuPanel;

    void Awake()
    {
        // Vérifie si une instance existe déjà
        if (instance == null)
        {
            instance = this;

            // Garde ce UI même si on change de scène
            DontDestroyOnLoad(gameObject);

            // Quand une nouvelle scène charge → on reset UI
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // détruit les doublons
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // cache tous les menus au début
        HideAll();
    }

    // appelé automatiquement quand une scène est chargée
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HideAll();
    }

    // ===================== DEATH UI =====================
    public void ShowDeath()
    {
        HideAll();

        if (DeathMenuPanel != null)
            DeathMenuPanel.SetActive(true);

        // libère la souris
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // pause le jeu
        Time.timeScale = 0f;
    }

    // ===================== VICTORY UI =====================
    public void ShowVictory()
    {
        HideAll();

        if (VictoryMenuPanel != null)
            VictoryMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    // ===================== RESET UI =====================
    public void HideAll()
    {
        if (DeathMenuPanel != null)
            DeathMenuPanel.SetActive(false);

        if (VictoryMenuPanel != null)
            VictoryMenuPanel.SetActive(false);

        // relance le jeu
        Time.timeScale = 1f;
    }

    void OnDestroy()
    {
        // évite les erreurs quand objet détruit
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}