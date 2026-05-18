using UnityEngine;

// Ce script gère la clé du bateau que le joueur peut ramasser
public class BoatKey : MonoBehaviour
{
    // Variable globale : indique si le joueur possède la clé
    public static bool hasKey = false;

    [Header("Audio")]
    public AudioSource audioSource;   // Source audio (optionnel)
    public AudioClip keySound;        // Son joué quand on ramasse la clé

    // ===================== COLLISION =====================
    void OnTriggerEnter(Collider other)
    {
        // Vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        // Le joueur a maintenant la clé
        hasKey = true;

        Debug.Log(" Key collected!");

        // ===================== SON =====================
        if (audioSource != null && keySound != null)
        {
            AudioSource.PlayClipAtPoint(keySound, transform.position);
        }

        // ===================== OBJET DISPARAÎT =====================
        Destroy(gameObject);
    }
}