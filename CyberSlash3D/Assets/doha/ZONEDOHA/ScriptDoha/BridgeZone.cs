using UnityEngine;

public class BridgeZone : MonoBehaviour
{public BridgeController bridgeController;
    public InventoryManager inventory;
    public GameObject panel;

    private bool playerInZone = false;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    void Update()
    {
        if (!playerInZone) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("🔥 E pressé");

            if (inventory != null && inventory.hasBossKey)
            {
                Debug.Log("✅ Clé OK → ouverture");

                if (bridgeController != null)
                    bridgeController.OpenDoor();

                if (panel != null)
                    panel.SetActive(false);
            }
            else
            {
                Debug.Log("❌ Il te faut la clé !");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            if (panel != null)
                panel.SetActive(true);

            Debug.Log("👤 Joueur entré dans la zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            if (panel != null)
                panel.SetActive(false);

            Debug.Log("🚪 Joueur sorti de la zone");
        }
    }
}