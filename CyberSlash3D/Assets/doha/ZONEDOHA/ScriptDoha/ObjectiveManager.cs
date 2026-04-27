using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public int totalSystems = 2;
    private int activatedSystems = 0;

    public bool hasBoatKey = false;
    public GameObject boatKey;

    public void ActivateSystem()
    {
        activatedSystems++;
        Debug.Log("Systèmes activés: " + activatedSystems + "/" + totalSystems);

        if (activatedSystems >= totalSystems)
        {
            boatKey.SetActive(true);
            Debug.Log("Clé du bateau activée !");
        }
    }

    public void GetBoatKey()
    {
        hasBoatKey = true;
    }

    public bool AllSystemsActivated()
    {
        return activatedSystems >= totalSystems;
    }
}
