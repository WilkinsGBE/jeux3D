using UnityEngine;
using TMPro;
public class BridgeZone : MonoBehaviour
{
    public Door door;
    public InventoryManager inventory;
    public GameObject panel;

    // message UI
    public GameObject messagePanel;
    public TMP_Text messageText;
    private bool playerInZone = false;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = true;
        Debug.Log(" PLAYER ENTER BRIDGE ZONE");

        if (panel != null)
            panel.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInZone = false;
        Debug.Log("🚪 PLAYER EXIT BRIDGE ZONE");

        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (panel != null)
                panel.SetActive(false);

            Debug.Log("📴 Panel fermé (D)");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(" E PRESSÉ");

            if (inventory == null)
            {
                Debug.LogError("Inventory non assignée !");
                return;
            }

            if (!inventory.hasBossKey)
            {
                Debug.Log("Tu n’as pas la clé !");
                return;
            }

            if (door == null)
            {
                Debug.LogError(" door  non assigné !");
                return;
            }

            door.OpenDoor();

            Debug.Log(" PORTE OUVERTE");

            if (panel != null)
                panel.SetActive(false);
        }
    }
    //  afficher message
    void ShowMessage(string msg)
    {
        if (messagePanel == null || messageText == null) return;

        messagePanel.SetActive(true);
        messageText.text = msg;

        CancelInvoke();
        Invoke(nameof(HideMessage), 5f);
    }

    void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}