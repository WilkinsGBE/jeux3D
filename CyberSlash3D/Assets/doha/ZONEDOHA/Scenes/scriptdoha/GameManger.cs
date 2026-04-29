using UnityEngine;

// Gestion globale du jeu (score, clés, UI, menus)
public class GameManager : MonoBehaviour
{
    // Singleton pour accès global
    public static GameManager instance;

    [Header("UI")]
    public UIManager ui;                 // interface HUD
    public PersistentUI persistentUI;   // menus mort / victoire

    [Header("Game Stats")]
    public int score = 0; // score joueur
    public int keys = 0;  // nombre de clés collectées
    // public int terminalsActivated = 0; // (désactivé)

    void Awake()
    {
        // setup singleton
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("GAME MANAGER START");

        // vérifie UI
        if (ui == null)
            Debug.LogError("UI MANAGER NON ASSIGNÉ !");
        else
            Debug.Log("UI MANAGER OK");

        // vérifie menus persistants
        if (persistentUI == null)
            Debug.LogError("PERSISTENT UI NON ASSIGNÉ !");
        else
            Debug.Log("PERSISTENT UI OK");

        UpdateHUD();
    }

    void Update()
    {
        // test score
        if (Input.GetKeyDown(KeyCode.J))
            AddScore(10);

        // test clé
        if (Input.GetKeyDown(KeyCode.K))
            AddKey();

        // afficher écran mort (test)
        if (Input.GetKeyDown(KeyCode.O))
            ShowDeathScreen();

        // afficher écran victoire (test)
        if (Input.GetKeyDown(KeyCode.I))
            ShowVictoryScreen();

        // reset UI menu
        if (Input.GetKeyDown(KeyCode.R) && persistentUI != null)
            persistentUI.HideAll();
    }

    // ===================== SCORE =====================
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        // update UI
        if (ui != null)
            ui.SetScore(score);
    }

    // ===================== KEYS =====================
    public void AddKey()
    {
        keys++;
        Debug.Log("Keys: " + keys);

        // update UI
        if (ui != null)
            ui.SetKeys(keys);
    }

    // ===================== DEATH SCREEN =====================
    public void ShowDeathScreen()
    {
        Debug.Log("PLAYER DEAD");

        if (persistentUI != null)
            persistentUI.ShowDeath();
    }

    // ===================== VICTORY SCREEN =====================
    public void ShowVictoryScreen()
    {
        Debug.Log("VICTORY");

        if (persistentUI != null)
            persistentUI.ShowVictory();
    }

    // ===================== UPDATE HUD =====================
    void UpdateHUD()
    {
        if (ui == null) return;

        ui.SetScore(score);
        ui.SetKeys(keys);
    }

    // ===================== WIN GAME =====================
    public void WinGame()
    {
        Debug.Log("VICTORY");

        if (persistentUI != null)
            persistentUI.ShowVictory();

        Time.timeScale = 0f; // pause jeu
    }
}