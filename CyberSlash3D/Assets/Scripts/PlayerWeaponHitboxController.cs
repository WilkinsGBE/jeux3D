using UnityEngine;

public class PlayerWeaponHitboxController : MonoBehaviour
{
    public GameObject weaponHitbox;
    private PlayerDamageHitbox damageHitbox;

    private void Awake()
    {
        if (weaponHitbox == null)
        {
            Debug.LogError("Weapon Hitbox is not assigned!");
            return;
        }

        damageHitbox = weaponHitbox.GetComponent<PlayerDamageHitbox>();

        if (damageHitbox == null)
        {
            Debug.LogError("PlayerDamageHitbox script is missing on weaponHitbox!");
            return;
        }

        weaponHitbox.SetActive(false);
    }

    
    public void SetAttack1()
    {
        if (damageHitbox == null) return;

        damageHitbox.SetDamage(25);
        damageHitbox.ResetHitbox();
        weaponHitbox.SetActive(true);
    }

    public void SetAttack2()
    {
        if (damageHitbox == null) return;

        damageHitbox.SetDamage(100);
        damageHitbox.ResetHitbox();
        weaponHitbox.SetActive(true);
    }

    public void DisableWeaponHitbox()
    {
        if (weaponHitbox == null) return;

        weaponHitbox.SetActive(false);
    }
}