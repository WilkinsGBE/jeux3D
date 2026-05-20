using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PersistentUI : MonoBehaviour
{
    // Singleton (une seule instance qui reste entre les scènes)
    public static PersistentUI instance;

    [Header("UI Panels")]
    public GameObject DeathMenuPanel;
    public GameObject VictoryMenuPanel;

    [Header("Victory Player Name")]
    public TMP_Text victoryPlayerNameText;

    [Header("Victory Score")]
    public TMP_Text victoryScoreText;

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

        string playerName =
            PlayerPrefs.GetString("PlayerName", "Joueur");

        if (victoryPlayerNameText != null)
            victoryPlayerNameText.text =
                "Bravo " + playerName + " !";

        int totalScore =
            PlayerPrefs.GetInt("Total2DScore", 0);

        if (victoryScoreText != null)
            victoryScoreText.text =
                "Score Total : " + totalScore;

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