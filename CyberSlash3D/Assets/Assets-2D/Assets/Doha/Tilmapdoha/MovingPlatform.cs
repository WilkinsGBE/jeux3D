using UnityEngine;

// Ce script fait bouger une plateforme entre deux points (A et B)
public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;

    public Transform pointB;

    public float moveSpeed = 2f;

    private Vector3 target;

    void Start()
    {
        // Au début, la plateforme va vers point B
        target = pointB.position;
    }

    void Update()
    {
        // Déplacement fluide vers la cible (A ou B)
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        // Si la plateforme arrive près de la cible
        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            // On change de direction :
            // si on était sur B → on va vers A
            // si on était sur A → on va vers B
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    // Quand le joueur monte sur la plateforme
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie que c’est bien le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            // On "attache" le joueur à la plateforme
            // comme ça il bouge avec elle
            collision.transform.SetParent(transform);
        }
    }

    // Quand le joueur quitte la plateforme
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Vérifie que c’est bien le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            // On détache le joueur de la plateforme
            collision.transform.SetParent(null);
        }
    }
}