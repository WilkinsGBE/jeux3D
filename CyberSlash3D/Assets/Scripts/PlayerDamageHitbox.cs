using UnityEngine;
using System.Collections.Generic;

public class PlayerDamageHitbox : MonoBehaviour
{
    public int damageAmount = 25;
    public string enemyLayerName = "enemy";

    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    private void OnEnable()
    {
        hitTargets.Clear();
    }

    public void ResetHitbox()
    {
        hitTargets.Clear();
    }

    public void SetDamage(int newDamage)
    {
        damageAmount = newDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable == null)
            return;

        GameObject targetObject = ((MonoBehaviour)damageable).gameObject;

        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);

        if (targetObject.layer != enemyLayer && other.gameObject.layer != enemyLayer)
            return;

        if (hitTargets.Contains(damageable))
            return;

        damageable.TakeDamage(damageAmount);
        hitTargets.Add(damageable);
    }
}