using UnityEngine;

public class PlayerDamageHitbox : MonoBehaviour
{
    public int damageAmount = 25;

    private bool hasHit = false;

    private void OnEnable()
    {
        hasHit = false;
    }

    public void ResetHitbox()
    {
        hasHit = false;
    }

    public void SetDamage(int newDamage)
    {
        damageAmount = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (!other.CompareTag("Boss") && !other.CompareTag("enemy")) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            hasHit = true;
        }

    }
}