using UnityEngine;

// Ce script gère la victoire quand le joueur entre dans le bateau
public class Boat : MonoBehaviour
{
    // ===================== COLLISION =====================
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        // Vérifie si le joueur possède la clé
        if (BoatKey.hasKey)
        {
            Debug.Log("WIN GAME");

            if (GameManager.instance != null)
                GameManager.instance.WinGame();
        }
        else
        {
            Debug.Log("Tu n'as pas la clé !");
        }
    }
}