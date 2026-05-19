using UnityEngine;

// Ce script gère la récupération d’un nombre dans le puzzle
public class NumberPickup : MonoBehaviour
{
    public int value;

    private bool playerNear;

    private bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si ce n’est pas le joueur → on ignore
        if (!other.CompareTag("Player")) return;

        // Le joueur est maintenant proche du nombre
        playerNear = true;

        Debug.Log(" Player near number " + value);
    }

    // Quand le joueur sort de la zone du trigger
    void OnTriggerExit2D(Collider2D other)
    {
        // Si ce n’est pas le joueur → on ignore
        if (!other.CompareTag("Player")) return;

        // Le joueur n’est plus proche
        playerNear = false;
    }

    void Update()
    {
        // Si le joueur n’est pas proche OU déjà utilisé → on ne fait rien
        if (!playerNear || used) return;

        // Si le joueur appuie sur E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Sécurité : vérifier que le GameManager existe
            if (GameManager2D.instance == null)
            {
                Debug.LogError("GameManager NULL");
                return;
            }

            // Marque ce nombre comme déjà utilisé
            used = true;

            Debug.Log(" PICKED NUMBER = " + value);

            // Envoie la valeur au GameManager pour vérifier l’ordre
            GameManager2D.instance.AddNumber(value);

            // Désactive le collider pour éviter les doubles interactions
            GetComponent<Collider2D>().enabled = false;

            // Cache l’objet dans la scène
            gameObject.SetActive(false);
        }
    }
}