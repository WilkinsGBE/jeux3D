using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
       [Header("Game System")]
    public Zone3GameManager gameManager;
    public EnemySpawner spawner;
    public int enemiesToSpawn = 5;
    public Transform spawnPoint;

    [Header("Interaction")]
    private bool playerInZone = false;
    private bool activated = false;

    [Header("UI")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    [Header("FX")]
    public ParticleSystem activateEffect;
    public AudioSource audioSource;
    public AudioClip activateSound;

    void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = true;

        if (!activated)
            ShowMessage("Appuie sur E pour activer le terminal");
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = false;

        HideMessage();
    }

    void Update()
    {
        if (!playerInZone || activated) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ActivateTerminal();
        }
    }

    void ActivateTerminal()
    {
        activated = true;

        //  Game logic
        if (gameManager != null)
            gameManager.ActivateTerminal();

        if (spawner != null && spawnPoint != null)
            spawner.SpawnEnemies(enemiesToSpawn, spawnPoint);

        //  FX VISUEL
        if (activateEffect != null)
            activateEffect.Play();

        //  SON
        if (audioSource != null && activateSound != null)
            audioSource.PlayOneShot(activateSound);

        Debug.Log("⚡ Terminal activé");

        HideMessage();
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
}