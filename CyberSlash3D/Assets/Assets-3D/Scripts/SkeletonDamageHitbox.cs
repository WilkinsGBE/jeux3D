using UnityEngine;

public class DamageHitbox : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Player";

    private bool hasHit = false;

    public void ResetHitbox()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        if (!other.CompareTag(targetTag)) return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            hasHit = true;
        }
    }
}