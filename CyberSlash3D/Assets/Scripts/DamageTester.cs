using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth.TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            playerHealth.Heal(10);
        }
    }
}