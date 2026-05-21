using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager2D : MonoBehaviour
{



    //
    public static GameManager2D instance;
    public GameObject player;
    public TMP_Text Score;
    public TMP_Text Timer;
    public TMP_Text resultBaseScoreText;
    public TMP_Text resultTimeText;
    public TMP_Text resultBonusText;
    public TMP_Text resultFinalScoreText;
    public AudioSource audioSource;
    public AudioClip deathSound;
    private bool isDead = false;

    [Header("UI")]
    public GameObject levelCompletePanel;


    [Header("Objectives")]
    public bool hasAccessCard;
    public int enemiesRemaining;

    private bool levelCompleted = false;

    [Header("Score")]
    public int score = 0;
    public int coinScore = 10;
    public float referenceTime = 300f;
    public int maxTimeBonus = 1000;
    private int baseScore;
    private float finalMultiplier;
    private int finalScore;
    private const string TotalScoreKey = "Total2DScore";

    [Header("Timer")]
    public float timer = 0f;
    private bool timerRunning = false;

    [Header("Enemy Tracking")]
    public int demonsKilled = 0;     // utilisé pour ouvrir le mur
    public bool wallUnlocked = false;

    [Header("Environment")]
    public GameObject laserWall;

    [Header("Puzzle (texte magique)")]
    public bool puzzleSolved = false;
    public string currentWord = "";
    public string correctWord = "LASER";

    public AngleZone angleZone;
    public Transform playerTransform;

    [Header("Number Puzzle (ordre 2-4-6-8)")]
    public int[] correctOrder = { 2, 4, 6, 8 };
    private int index = 0;

    [Header("Boss / Angel")]
    public GameObject angle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }
    void Start()
    {
        Time.timeScale = 0f; // Pause lorsque jeu est lance

    }

    // A REVISER (RESPAWN)

    public void PlayerDied()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Player died");

        timerRunning = false;

        // Son Joueur mort
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (PersistentUI2D.instance == null)
        {
            Debug.LogError("UIManager instance is null");
            return;
        }

        if (PersistentUI2D.instance.playerDeadPanel == null)
        {
            Debug.LogError("playerDeadPanel is not assigned in UIManager");
            return;
        }

        PersistentUI2D.instance.playerDeadPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void collectCard()
    {
        hasAccessCard = true;
        Debug.Log("Carte collecte");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Time.timeScale = 0f;
            timerRunning = false;
            return;
        }

        Time.timeScale = 1f;

        score = PlayerPrefs.GetInt(TotalScoreKey, 0);

        if (Score != null)
            Score.text = "Score : " + score;

        hasAccessCard = false;
        levelCompleted = false;
        isDead = false;

        timer = 0f;
        timerRunning = true;

        enemiesRemaining = GameObject.FindGameObjectsWithTag("enemies").Length;
        Debug.Log("Enemies in level: " + enemiesRemaining);

        // Scene sans carte

        if (scene.name == "WilkinsScene 1")
        {
            hasAccessCard = true;
        }
        else
        {
            hasAccessCard = false;
        }
    }

    public void CheckWinConditions()
    {
        if (levelCompleted)
            return;

        if (hasAccessCard && enemiesRemaining <= 0)
        {
            LevelComplete();
        }
    }

    public void LevelComplete()
    {
        levelCompleted = true;
        timerRunning = false;

        baseScore = score;

        finalMultiplier = CalculateTimeMultiplier();

        finalScore = Mathf.RoundToInt(baseScore * finalMultiplier);

        int previousTotal = PlayerPrefs.GetInt(TotalScoreKey, 0);

        score = previousTotal + finalScore;

        PlayerPrefs.SetInt(TotalScoreKey, score);
        PlayerPrefs.Save();

        if (Score != null)
            Score.text = "Score : " + score;

        ShowScoreBreakdown();

        Debug.Log("Level complete");

        PersistentUI2D.instance.LevelCompletePanel.SetActive(true);

        Time.timeScale = 0f;
    }


    // Score

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        if (Score != null)
            Score.text = "Score : " + score;
    }

    public void CoinCollected()
    {
        AddScore(coinScore);
    }


    // Timer
    void UpdateTimerUI()
    {
        if (Timer == null)
            return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Bonus selon le temps
    public float CalculateTimeMultiplier()
    {
        float timePercent = Mathf.Clamp01(1f - (timer / 300f));

        // multiplier between 1x and 2x
        float multiplier = 1f + timePercent;

        Debug.Log("Time multiplier: " + multiplier);
        return multiplier;
    }

    // Montre le calcul du score bonus
    void ShowScoreBreakdown()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        float timePercent = Mathf.Clamp01(1f - (timer / referenceTime)) * 100f;

        if (resultBaseScoreText != null)
            resultBaseScoreText.text = "Score avant bonus: " + baseScore;

        if (resultTimeText != null)
            resultTimeText.text = "Time : " + string.Format("{0:00}:{1:00}", minutes, seconds);

        if (resultBonusText != null)
            resultBonusText.text = "Time Bonus : " + timePercent.ToString("0") + "%  (x" + finalMultiplier.ToString("0.00") + ")";

        if (resultFinalScoreText != null)
            resultFinalScoreText.text = "Score Final: " + finalScore;
        ShowScoreDetails();
    }

    public void ShowScoreDetails()
    {
        resultBaseScoreText.gameObject.SetActive(true);
        resultTimeText.gameObject.SetActive(true); ;
        resultBonusText.gameObject.SetActive(true); ;
        resultFinalScoreText.gameObject.SetActive(true); ;
    }



    public void DemonKilled()
    {
        demonsKilled++;

        if (demonsKilled >= 2 && !wallUnlocked)
        {
            wallUnlocked = true;
            Debug.Log("Wall unlocked!");
        }
    }
    public void ActivateLaserWall()
    {
        if (laserWall != null)
        {
            laserWall.SetActive(true);
            Debug.Log("Laser wall activated (interaction possible)");
        }
    }
    public void CheckPuzzle()
    {
        if (!puzzleSolved && currentWord == correctWord)
        {
            puzzleSolved = true;

            Debug.Log("Puzzle solved → Angel should become vulnerable");
        }
    }



    public void AddNumber(int value)
    {
        if (index >= correctOrder.Length)
            return;

        Debug.Log("EXPECTED = " + correctOrder[index] + " | GOT = " + value + " | INDEX = " + index);

        if (correctOrder[index] == value)
        {
            index++;

            if (index >= correctOrder.Length)
            {
                StartCoroutine(WinSequence());
                index = 0;
            }
        }
        else
        {
            Debug.Log("WRONG ORDER → PLAYER DIES");
            index = 0;
            PlayerDied();
        }
    }
    private System.Collections.IEnumerator WinSequence()
    {
        Debug.Log("PUZZLE COMPLETE → ANGEL DEAD");

        if (angle != null)
        {
            Angel script = angle.GetComponent<Angel>();

            if (script != null)
                script.enabled = false; // stop IA

            Destroy(angle);
            angle = null;
        }

        yield return new WaitForSeconds(1f);

        LevelComplete();
    }
}
