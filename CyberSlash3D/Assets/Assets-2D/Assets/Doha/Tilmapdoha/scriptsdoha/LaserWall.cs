using UnityEngine;
using TMPro;
//gere le laser wall
public class LaserWall : MonoBehaviour
{
    private bool playerInZone = false;

    [Header("UI")]
    public GameObject messageUI;

    public TextMeshProUGUI messageText;

    void Start()
    {
        // Cache le message au départ
        if (messageUI != null)
            messageUI.SetActive(false);
    }

    void Update()
    {
        // Si le joueur n’est pas dans la zone ou GameManager absent → rien faire
        if (!playerInZone || GameManager2D.instance == null)
            return;

        // 🔴 CONDITION 1 : pas assez d’ennemis tués
        if (GameManager2D.instance.demonsKilled < 2)
        {
            messageUI.SetActive(true);
            messageText.text = "Tu dois tuer 2 ennemis pour passer !";
            return;
        }

        // 🟢 CONDITION 2 : joueur a assez de kills
        messageUI.SetActive(true);
        messageText.text = "Appuie sur E pour ouvrir la porte";

        // Si le joueur appuie sur E → ouverture du mur
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenWall();
        }
    }

    void OpenWall()
    {
        Debug.Log("Laser wall opened");

        // Cache UI
        if (messageUI != null)
            messageUI.SetActive(false);

        // Désactive le mur entier (parent)
        transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Détecte entrée du joueur dans la zone
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Détecte sortie du joueur
        if (other.CompareTag("Player"))
        {
            playerInZone = false;

            // Cache le message quand il sort
            if (messageUI != null)
                messageUI.SetActive(false);
        }
    }
}