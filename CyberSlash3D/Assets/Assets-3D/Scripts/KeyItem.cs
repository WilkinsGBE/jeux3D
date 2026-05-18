using UnityEngine;

/// <summary>
/// Clé que le joueur peut ramasser dans le labyrinthe.
/// Quand toutes les clés sont ramassées, la sortie s'ouvre.
/// </summary>
public class KeyItem : MonoBehaviour
{
    void Start()
    {
        // Enregistre cette clé auprès du gestionnaire de jeu
        GestionnaireJeu.EnregistrerCle();

        // Ajoute un collider trigger si le prefab n'en a pas
        var col = GetComponent<Collider>();
        if (col == null)
        {
            var sphere = gameObject.AddComponent<SphereCollider>();
            sphere.radius    = 0.8f;
            sphere.isTrigger = true;
        }
        else
        {
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider autre)
    {
        // Vérifie que c'est bien le joueur qui touche la clé
        if (!autre.CompareTag("Player")) return;

        // Informe le gestionnaire qu'une clé est ramassée
        GestionnaireJeu.RamasserCle();

        // Détruit la clé
        Destroy(gameObject);
    }
}
