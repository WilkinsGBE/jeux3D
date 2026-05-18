using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    [Header("Death Menu")]
    public GameObject deathMenu; // panel de mort affiché quand le joueur tombe dans l'eau

    private void OnTriggerEnter(Collider other)
    {
        // vérifie si c’est le joueur
        if (!other.CompareTag("Player")) return;

        // récupère le script de vie du joueur
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // kill instant du joueur (dégâts énormes)
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(9999);
        }

        // affiche menu mort
        ShowDeathMenu();
    }

    void ShowDeathMenu()
    {
        // active le menu de mort
        if (deathMenu != null)
            deathMenu.SetActive(true);

        // pause le jeu
        Time.timeScale = 0f;

        // libère la souris
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("💀 Player died in water");
    }
}