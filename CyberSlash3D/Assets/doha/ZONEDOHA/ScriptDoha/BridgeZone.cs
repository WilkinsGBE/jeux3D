using UnityEngine;
using TMPro;

// Ce script gère la zone "Bridge" : UI, interaction porte, et affichage info joueur
public class BridgeZone : MonoBehaviour
{
    // Porte à ouvrir
    public Door door;

    // Inventaire du joueur (clé requise)
    public InventoryManager inventory;

    [Header("UI Panels")]
    public GameObject panel;         // Panel principal (info Bridge)
    public GameObject messagePanel;  // Panel message (E / D / erreur)

    [Header("UI Text")]
    public TMP_Text messageText;       // Texte instructions
    public TMP_Text descriptionText;   // Texte description (DOHA info)

    private bool playerInZone = false; // Vérifie si joueur est dans la zone

    public ZombieAi[] zombies; // (pas utilisé ici mais prévu pour interaction future)

    // ===================== START =====================
    void Start()
    {
        // Cache le panel principal au début
        if (panel != null)
            panel.SetActive(false);

        // Cache le panel message
        if (messagePanel != null)
            messagePanel.SetActive(false);

        // Cache la description DOHA
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(false);
    }

    // ===================== PLAYER ENTER ZONE =====================
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        playerInZone = true;

        Debug.Log(" PLAYER ENTER BRIDGE ZONE");

        // Affiche le panel principal (info DOHA)
        if (panel != null)
            panel.SetActive(true);

        // Affiche la description DOHA
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(true);

        // Affiche message d’interaction
        ShowMessage("Appuie sur E pour ouvrir la porte\nD pour fermer");
    }

    // ===================== PLAYER EXIT ZONE =====================
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = false;

        Debug.Log(" PLAYER EXIT BRIDGE ZONE");

        // Cache panel principal
        if (panel != null)
            panel.SetActive(false);

        // Cache description DOHA
        if (descriptionText != null)
            descriptionText.gameObject.SetActive(false);

        // Cache message UI
        HideMessage();
    }

    // ===================== UPDATE INPUT =====================
    void Update()
    {
        // Si joueur n’est pas dans la zone → rien faire
        if (!playerInZone) return;

        // Touche D → fermer panel
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (panel != null)
                panel.SetActive(false);

            Debug.Log("Panel fermé (D)");
        }

        // Touche E → interaction porte
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E PRESSÉ");

            // Vérifie inventaire
            if (inventory == null)
            {
                Debug.LogError("Inventory non assignée !");
                return;
            }

            // Vérifie clé
            if (!inventory.hasBossKey)
            {
                ShowMessage("Tu n'as pas la clé !");
                return;
            }

            // Vérifie porte
            if (door == null)
            {
                Debug.LogError("Door non assignée !");
                return;
            }

            // Ouvre la porte
            door.OpenDoor();

            Debug.Log(" PORTE OUVERTE");

            // Cache panel après ouverture
            if (panel != null)
                panel.SetActive(false);
        }
    }

    // ===================== MESSAGE UI =====================

    // Affiche un message temporaire
    void ShowMessage(string msg)
    {
        if (messagePanel == null || messageText == null) return;

        messagePanel.SetActive(true);
        messageText.text = msg;

        // Cache après 3 secondes
        CancelInvoke();
        Invoke(nameof(HideMessage), 3f);
    }

    // Cache message UI
    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}