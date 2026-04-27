using UnityEngine;
//doha
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIManager ui;
    public PersistentUI persistentUI;

    public float health = 1f;
    public float energy = 1f;
    public int score = 0;
    public int keys = 0;
    public int terminalsActivated = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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


        if (Input.GetKeyDown(KeyCode.H))
            TakeDamage(0.1f);

        if (Input.GetKeyDown(KeyCode.J))
            AddScore(10);

        if (Input.GetKeyDown(KeyCode.K))
            AddKey();

        if (Input.GetKeyDown(KeyCode.C))
            ActivateTerminal();


        if (Input.GetKeyDown(KeyCode.P))
            persistentUI.ShowPause();

        if (Input.GetKeyDown(KeyCode.O))
            persistentUI.ShowDeath();

        if (Input.GetKeyDown(KeyCode.I))
            persistentUI.ShowVictory();

        if (Input.GetKeyDown(KeyCode.R))
            persistentUI.HideAll();
    }



    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp01(health);

        Debug.Log("💔 Damage taken: " + dmg + " | HP: " + health);

        if (ui != null)
            ui.SetHealth(health);

        if (health <= 0)
        {
            Debug.Log("☠ PLAYER DEAD");
            persistentUI.ShowDeath();
        }
    }


    public void UseEnergy(float amount)
    {
        energy -= amount;
        energy = Mathf.Clamp01(energy);

        if (ui != null)
            ui.SetEnergy(energy);
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

    void UpdateHUD()
    {
        if (ui == null) return;

        ui.SetHealth(health);
        ui.SetEnergy(energy);
        ui.SetScore(score);
        ui.SetKeys(keys);
        ui.SetTerminals(terminalsActivated);
    }
}