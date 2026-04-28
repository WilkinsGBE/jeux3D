using UnityEngine;

public class Zone3GameManager : MonoBehaviour
{
[Header("Systems")]
    public int totalTerminals = 2;
    private int activatedTerminals = 0;

    [Header("Boat Key")]
    public GameObject boatKey;
    public bool hasBoatKey = false;

    [Header("Victory")]
    public GameObject victoryPanel;

    // 🔵 Terminal activation
    public void ActivateTerminal()
    {
        activatedTerminals++;

        Debug.Log("Terminal activé: " + activatedTerminals + "/" + totalTerminals);

        if (activatedTerminals >= totalTerminals)
        {
            SpawnBoatKey();
        }
    }

    // 🔵 Spawn clé bateau
    void SpawnBoatKey()
    {
        if (boatKey != null)
        {
            boatKey.SetActive(true);
            Debug.Log("🔑 Clé du bateau activée !");
        }
    }

    // 🔵 Ramasser clé bateau
    public void CollectBoatKey()
    {
        hasBoatKey = true;
        Debug.Log("🔑 Clé bateau récupérée !");
    }

    // 🔵 Victoire
    public void TryVictory()
    {
        if (hasBoatKey)
        {
            Debug.Log("🏆 VICTOIRE !");

            if (victoryPanel != null)
                victoryPanel.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("❌ Tu n'as pas la clé !");
        }
    }
}
