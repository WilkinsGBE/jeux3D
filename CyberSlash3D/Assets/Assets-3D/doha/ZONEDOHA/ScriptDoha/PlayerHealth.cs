using UnityEngine;

public class playerhealt : MonoBehaviour
{
 public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Vie joueur: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over");
    }
}
