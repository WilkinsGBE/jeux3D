using UnityEngine;

public class fireball : MonoBehaviour
{
    public float speed = 6f;

    public float damage = 25f;

    public float lifeTime = 3f;

    private Vector2 direction;

    void Start()
    {
        // Détruit automatiquement la fireball après un certain temps
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Déplace la fireball dans la direction donnée
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Définit la direction de la fireball (appelé par l’ennemi)
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la fireball touche le joueur
        if (collision.CompareTag("Player"))
        {
            // Essaie de trouver un HealthManager sur le joueur
            HealthManager hm = collision.GetComponentInParent<HealthManager>();

            if (hm != null)
            {
                hm.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            // Sinon essaie un autre système de vie
            PlayerHealth ph = collision.GetComponentInParent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(Mathf.RoundToInt(damage)); ;
                Destroy(gameObject);
                return;
            }

            // Si aucun système trouvé → détruit quand même la fireball
            Destroy(gameObject);
        }

        // Si la fireball touche le sol → elle disparaît
        if (collision.CompareTag("Ground"))
            Destroy(gameObject);
    }
}