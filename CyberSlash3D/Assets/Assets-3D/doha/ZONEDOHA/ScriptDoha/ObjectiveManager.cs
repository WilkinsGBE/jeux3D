using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [Header("Objectif du jeu")]
    public int totalSystems = 2; // Nombre total de systèmes à activer
    private int activatedSystems = 0; // Compteur des systèmes activés

    [Header("Clé du bateau")]
    public bool hasBoatKey = false; // Est-ce que le joueur a la clé ?
    public GameObject boatKey; // Objet clé dans la scène

    // ===================== ACTIVER UN SYSTÈME =====================
    public void ActivateSystem()
    {
        // augmente le compteur
        activatedSystems++;

        Debug.Log("Systèmes activés: " + activatedSystems + "/" + totalSystems);

        // si tous les systèmes sont activés
        if (activatedSystems >= totalSystems)
        {
            // on active la clé du bateau dans la scène
            if (boatKey != null)
                boatKey.SetActive(true);

            Debug.Log("Clé du bateau activée !");
        }
    }

    // ===================== RAMASSER LA CLÉ =====================
    public void GetBoatKey()
    {
        hasBoatKey = true;
    }

    // ===================== VÉRIFIER SI TOUT EST FAIT =====================
    public bool AllSystemsActivated()
    {
        return activatedSystems >= totalSystems;
    }
}