using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    GameManager2D gameManager;
    public Image HP_Fill;
    public float healthAmount;
    public float maxHealth;
    private float lastDamageTime;
    public float damageCooldown = 0.5f;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindFirstObjectByType<GameManager2D>();
        maxHealth = healthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        HP_Fill.fillAmount = Mathf.Clamp(healthAmount / maxHealth, 0, 1);
        if (healthAmount <= 0)
        {
            gameManager.PlayerDied(); // A REVISER (RESPAWN)
        }
    }

    public void TakeDamage(float damage)
    {
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        healthAmount -= damage;
        anim.SetTrigger("isHurt");
        lastDamageTime = Time.time;
    }

}
