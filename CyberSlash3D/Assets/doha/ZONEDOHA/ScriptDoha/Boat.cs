using UnityEngine;

// Ce script gère la victoire quand le joueur entre dans le bateau
public class Boat : MonoBehaviour
{
    [Header("Victory UI")]
    public GameObject victoryMenu; // Menu de victoire affiché à la fin du jeu

    // ===================== COLLISION =====================
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        // Vérifie si le joueur possède la clé
        if (BoatKey.hasKey)
        {
            Debug.Log("WIN GAME");

            // Affiche le menu de victoire
            if (victoryMenu != null)
                victoryMenu.SetActive(true);

            // Pause du jeu
            Time.timeScale = 0f;

            // Débloque la souris
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Si pas de clé
            Debug.Log(" Tu n'as pas la clé !");
        }
    }
}