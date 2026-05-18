using UnityEngine;

public class Zone3GameManager : MonoBehaviour
{ public static Zone3GameManager instance;

    [Header("Systems")]
    public int totalTerminals = 2;
    private int activatedTerminals = 0;

    //  [Header("Score")]
    // public int score = 0;

    [Header("Boat Key")]
    public GameObject boatKey;
    public bool hasBoatKey = false;

   /* [Header("Victory")]
    public GameObject victoryMenuPanel;
*/

    void Awake()
    {
             instance = this;

    }
    //  Terminal activation
    public void ActivateTerminal()
    {
        activatedTerminals++;

        Debug.Log("Terminal activé: " + activatedTerminals + "/" + totalTerminals);

        if (activatedTerminals >= totalTerminals)
        {
            SpawnBoatKey();
        }
    }

    //  Spawn clé bateau
    void SpawnBoatKey()
    {
        if (boatKey != null)
        {
            boatKey.SetActive(true);
            Debug.Log(" Clé du bateau activée !");
        }
    }

    //  Ramasser clé bateau
    public void CollectBoatKey()
    {
        hasBoatKey = true;
        Debug.Log(" Clé bateau récupérée !");
    }
  
    public bool CanWin()
    {
        return hasBoatKey;
    }
}
