using UnityEngine;

public class BossFistHitboxController : MonoBehaviour
{
    public GameObject fistHitbox;

    private Collider fistCollider;
    private SkeletonDamageHitbox damageHitbox;

    private void Awake()
    {
        if (fistHitbox == null)
        {
            Debug.LogError("Fist Hitbox is not assigned!");
            return;
        }

        fistCollider = fistHitbox.GetComponent<Collider>();
        damageHitbox = fistHitbox.GetComponent<SkeletonDamageHitbox>();

        if (fistCollider != null)
            fistCollider.enabled = false;
    }

    public void EnableFist()
    {
        if (damageHitbox != null)
            damageHitbox.ResetSkeletonHitbox();

        if (fistCollider != null)
            fistCollider.enabled = true;
    }

    public void DisableFist()
    {
        if (fistCollider != null)
            fistCollider.enabled = false;
    }
}