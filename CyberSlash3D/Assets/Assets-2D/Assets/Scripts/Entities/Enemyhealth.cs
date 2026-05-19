using Unity.VisualScripting;
using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    public float health;
    public float currentHealth;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < currentHealth)
        {
            currentHealth = health;
            anim.SetTrigger("Attacked");
        }
        if (health <= 0)
        {
            anim.SetBool("isDead", true);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<MoveEnnemy>().enabled = false;
            Debug.Log("Enemy is dead");
        }
    }

    public void DestroyEnemy()
    {
        GameManager2D.instance.enemiesRemaining--;
        GameManager2D.instance.AddScore(75);
        GameManager2D.instance.CheckWinConditions();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
        }
    }
}
