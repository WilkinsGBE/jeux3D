using UnityEngine;
//gere la zone death
public class DeathZone : MonoBehaviour
{
    // Détecte quand un objet entre dans la zone de mort (trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si c’est le joueur
        if (other.CompareTag("Player"))
        {
            if (GameManager2D.instance != null)
            {
                // Tue le joueur
                GameManager2D.instance.PlayerDied();
            }
        }
    }
}