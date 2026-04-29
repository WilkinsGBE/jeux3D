using UnityEngine;

public class SkeletonDamageHitbox : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Player";

    private bool hasHit = false;

    public void ResetSkeletonHitbox()
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