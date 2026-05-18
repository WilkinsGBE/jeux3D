using UnityEngine;

public class traps : MonoBehaviour
{
    public HealthManager PlayerHealth;
    public float damage = 75f;
    public bool upDown;

    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        if (upDown)
        {
            anim.SetBool("upDown", true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth.TakeDamage(damage);
        }

    }

}