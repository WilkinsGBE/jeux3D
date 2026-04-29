using System.Collections;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    [Header("Game System")]
    public EnemySpawner spawner; // spawn des ennemis
    public int enemiesToSpawn = 5;

    [Header("Interaction")]
    private bool playerInZone = false; // joueur dans la zone
    private bool activated = false;     // terminal déjà activé

    [Header("UI")]
    public GameObject messagePanel;     // panel message (E / T)
    public TextMeshProUGUI messageText;

    public TextMeshProUGUI terminalText;         // texte activation terminal
    public TextMeshProUGUI terminalCounterText;  // compteur UI

    [Header("FX")]
    public ParticleSystem activateEffect; // effet particule activation

    [Header("Audio")]
    public AudioSource audioSource; // audio source terminal
    public AudioClip activateSound; // son activation

    [Header("Objective")]
    public static int terminalCount = 0; // nombre total de terminals activés
    public int requiredTerminals = 2;    // objectif
    private bool keyGiven = false;       // clé déjà donnée

    [Header("Boat Key")]
    public GameObject boatKey; // clé bateau

    [Header("Camera FX")]
    public Camera playerCamera;   // caméra joueur
    public float shakeAmount = 0.08f;  // intensité shake
    public float shakeDuration = 0.2f; // durée shake

    void Start()
    {
        // cache UI au début
        if (messagePanel != null)
            messagePanel.SetActive(false);

        if (terminalText != null)
            terminalText.gameObject.SetActive(false);

        UpdateTerminalUI();
    }

    void OnTriggerEnter(Collider other)
    {
        // détecte joueur dans zone
        if (!other.CompareTag("Player")) return;

        playerInZone = true;

        if (!activated)
            ShowMessage("Appuie sur E pour activer le terminal\nAppuie sur T pour fermer");
    }

    void OnTriggerExit(Collider other)
    {
        // joueur sort de la zone
        if (!other.CompareTag("Player")) return;

        playerInZone = false;
        HideMessage();
    }

    void Update()
    {
        if (!playerInZone) return;

        // activation terminal
        if (!activated && Input.GetKeyDown(KeyCode.E))
        {
            ActivateTerminal();
        }

        // fermer message
        if (Input.GetKeyDown(KeyCode.T))
        {
            HideMessage();
        }
    }

    // ===================== ACTIVATION TERMINAL =====================
    void ActivateTerminal()
    {
        if (activated) return;

        activated = true;

        // augmente compteur terminal
        terminalCount++;
        UpdateTerminalUI();

        // spawn ennemis
        //if (spawner != null)
        //    spawner.SpawnEnemies(enemiesToSpawn);

        // effet particule
        if (activateEffect != null)
            activateEffect.Play();

        // son activation
        if (audioSource != null && activateSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
            audioSource.PlayOneShot(activateSound);
        }

        // shake caméra
        StartCoroutine(ShakeCamera());

        // message UI activation
        StartCoroutine(ShowTerminalActivationMessage());

        // clé bateau si objectif atteint
        if (terminalCount >= requiredTerminals && !keyGiven)
        {
            keyGiven = true;

            if (boatKey != null)
                boatKey.SetActive(true);

            StartCoroutine(ZoneUnlockedMessage());
        }

        HideMessage();
    }

    // ===================== UI COMPTEUR =====================
    void UpdateTerminalUI()
    {
        if (terminalCounterText != null)
        {
            terminalCounterText.text =
                "Terminal activé " + terminalCount + "/" + requiredTerminals;
        }
    }

    void ShowMessage(string msg)
    {
        if (messagePanel == null || messageText == null) return;

        messagePanel.SetActive(true);
        messageText.text = msg;
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    // ===================== MESSAGE ACTIVATION =====================
    IEnumerator ShowTerminalActivationMessage()
    {
        if (terminalText == null) yield break;

        terminalText.gameObject.SetActive(true);
        terminalText.text = $"Terminal {terminalCount}/{requiredTerminals}";

        yield return new WaitForSeconds(2f);

        terminalText.gameObject.SetActive(false);
    }

    // ===================== CAMERA SHAKE =====================
    IEnumerator ShakeCamera()
    {
        if (playerCamera == null) yield break;

        Vector3 originalPos = playerCamera.transform.localPosition;
        float t = 0f;

        while (t < shakeDuration)
        {
            playerCamera.transform.localPosition =
                originalPos + Random.insideUnitSphere * shakeAmount;

            t += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.localPosition = originalPos;
    }

    // ===================== MESSAGE CLÉ =====================
    IEnumerator ZoneUnlockedMessage()
    {
        yield return new WaitForSeconds(1f);

        if (terminalText != null)
        {
            terminalText.gameObject.SetActive(true);
            terminalText.text = "🗝️ Clé bateau obtenue !";
        }

        yield return new WaitForSeconds(2f);

        if (terminalText != null)
            terminalText.gameObject.SetActive(false);
    }

    // ===================== RESET =====================
    public static void ResetTerminals()
    {
        terminalCount = 0;
    }

    public void ResetTerminal()
    {
        activated = false;
        keyGiven = false;

        if (activateEffect != null)
            activateEffect.Stop();

        UpdateTerminalUI();
    }
}