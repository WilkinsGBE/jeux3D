using UnityEngine;

// Gestion globale du jeu (score, clés, UI, menus)
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public UIManager ui;
    public PersistentUI persistentUI;

    [Header("Game Stats")]
    public int score = 0;
    public int keys = 0;

    [Header("Ambient Music")]
    public AudioSource ambientMusicSource;

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

        score = PlayerPrefs.GetInt("Total2DScore", 0);

        if (ui == null)
            Debug.LogError("UI MANAGER NON ASSIGNÉ !");
        else
            Debug.Log("UI MANAGER OK");

        if (persistentUI == null)
            Debug.LogError("PERSISTENT UI NON ASSIGNÉ !");
        else
            Debug.Log("PERSISTENT UI OK");

        UpdateHUD();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            AddScore(10);

        if (Input.GetKeyDown(KeyCode.K))
            AddKey();

        if (Input.GetKeyDown(KeyCode.O))
            ShowDeathScreen();

        if (Input.GetKeyDown(KeyCode.I))
            ShowVictoryScreen();

        if (Input.GetKeyDown(KeyCode.R) && persistentUI != null)
            persistentUI.HideAll();
    }

    // ===================== AMBIENT MUSIC =====================
    public void PlayAmbientMusic()
    {
        if (ambientMusicSource != null && !ambientMusicSource.isPlaying)
            ambientMusicSource.Play();
    }

    public void StopAmbientMusic()
    {
        if (ambientMusicSource != null && ambientMusicSource.isPlaying)
            ambientMusicSource.Stop();
    }

    // ===================== SCORE =====================
    public void AddScore(int amount)
    {
        score += amount;
        PlayerPrefs.SetInt("Total2DScore", score);
        PlayerPrefs.Save();

        if (ui != null)
            ui.SetScore(score);
    }

    // ===================== KEYS =====================
    public void AddKey()
    {
        keys++;
        Debug.Log("Keys: " + keys);

        if (ui != null)
            ui.SetKeys(keys);
    }

    // ===================== DEATH SCREEN =====================
    public void ShowDeathScreen()
    {
        Debug.Log("PLAYER DEAD");

        StopAmbientMusic();

        if (persistentUI != null)
            persistentUI.ShowDeath();
    }

    // ===================== VICTORY SCREEN =====================
    public void ShowVictoryScreen()
    {
        Debug.Log("VICTORY");

        StopAmbientMusic();

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

        StopAmbientMusic();

        if (persistentUI != null)
            persistentUI.ShowVictory();

        Time.timeScale = 0f;
    }
}