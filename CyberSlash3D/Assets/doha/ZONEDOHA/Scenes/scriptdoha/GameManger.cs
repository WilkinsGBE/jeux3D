using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public UIManager ui;
    public PersistentUI persistentUI;

    [Header("Game Stats")]
    public int score = 0;
    public int keys = 0;
    public int terminalsActivated = 0;

    void Awake()
    {
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

        if (ui == null)
            Debug.LogError("❌ UI MANAGER NON ASSIGNÉ !");
        else
            Debug.Log("✅ UI MANAGER OK");

        if (persistentUI == null)
            Debug.LogError("❌ PERSISTENT UI NON ASSIGNÉ !");
        else
            Debug.Log("✅ PERSISTENT UI OK");

        UpdateHUD();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            AddScore(10);

        if (Input.GetKeyDown(KeyCode.K))
            AddKey();

        if (Input.GetKeyDown(KeyCode.C))
            ActivateTerminal();

        if (Input.GetKeyDown(KeyCode.O))
            ShowDeathScreen();

        if (Input.GetKeyDown(KeyCode.I))
            ShowVictoryScreen();

        if (Input.GetKeyDown(KeyCode.R) && persistentUI != null)
            persistentUI.HideAll();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("🧮 Score: " + score);

        if (ui != null)
            ui.SetScore(score);
    }

    public void AddKey()
    {
        keys++;
        Debug.Log("🔑 Keys: " + keys);

        if (ui != null)
            ui.SetKeys(keys);
    }

    public void ActivateTerminal()
    {
        terminalsActivated++;
        Debug.Log("🖥 Terminal activated: " + terminalsActivated);

        if (ui != null)
            ui.SetTerminals(terminalsActivated);
    }

    public void ShowDeathScreen()
    {
        Debug.Log("☠ PLAYER DEAD");

        if (persistentUI != null)
            persistentUI.ShowDeath();
    }

    public void ShowVictoryScreen()
    {
        Debug.Log("🏆 VICTORY");

        if (persistentUI != null)
            persistentUI.ShowVictory();
    }

    void UpdateHUD()
    {
        if (ui == null) return;

        ui.SetScore(score);
        ui.SetKeys(keys);
        ui.SetTerminals(terminalsActivated);
    }
}