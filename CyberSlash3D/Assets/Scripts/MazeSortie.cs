using UnityEngine;

/// <summary>
/// Zone de sortie du labyrinthe.
/// Se déverrouille quand toutes les clés sont ramassées.
/// </summary>
public class MazeSortie : MonoBehaviour
{
    private static MazeSortie instance;
    private bool estOuverte = false;

    void Awake()
    {
        instance  = this;
        estOuverte = false;

        // Ajoute un collider trigger si le prefab n'en a pas
        var col = GetComponent<Collider>();
        if (col == null)
        {
            var box = gameObject.AddComponent<BoxCollider>();
            box.size      = new Vector3(2f, 2f, 2f);
            box.isTrigger = true;
        }
        else
        {
            col.isTrigger = true;
        }
    }

    // Appelé par GestionnaireJeu quand toutes les clés sont ramassées
    public static void OuvrirSortie()
    {
        if (instance != null)
            instance.estOuverte = true;
    }

    void OnTriggerEnter(Collider autre)
    {
        if (!autre.CompareTag("Player")) return;

        if (!estOuverte)
        {
            Debug.Log("[Labyrinthe] Sortie verrouillée ! Ramasse toutes les clés d'abord.");
            return;
        }

        Debug.Log("[Labyrinthe] Tu as trouvé la sortie ! Bravo !");

        // TODO : charger la scène suivante ou afficher l'écran de victoire
        // SceneManager.LoadScene("SceneSuivante");
    }
}
