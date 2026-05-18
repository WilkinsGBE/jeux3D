using UnityEngine;

// Projectile lancé par un ennemi (ou boss)
public class Projectiledoha : MonoBehaviour
{
    public float speed = 6f;

    public float damage = 10f;

    public float lifeTime = 3f;

    private Vector2 direction;

    void Start()
    {
        // Détruit automatiquement le projectile après un certain temps
        Destroy(gameObject, lifeTime);
    }

    // Définit la direction vers la cible (ex: joueur)
    public void SetTarget(Vector2 targetDirection)
    {
        direction = targetDirection.normalized;
    }

    void Update()
    {
        // Déplacement du projectile dans la direction donnée
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le projectile touche le joueur
        if (collision.CompareTag("Player"))
        {
            // Récupère le système de vie du joueur
            HealthManager playerHealth = collision.GetComponentInParent<HealthManager>();

            // Applique les dégâts si le script existe
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);

            // Détruit le projectile après impact
            Destroy(gameObject);
        }

        // Si le projectile touche le sol
        if (collision.CompareTag("Ground"))
            Destroy(gameObject);
    }
}