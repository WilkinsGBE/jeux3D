using UnityEngine;

public class SkeletonWeaponHitboxController : MonoBehaviour
{
    public GameObject weaponHitbox;

    private SkeletonDamageHitbox damageHitbox;
    private bool weaponEnabled = false;

    private void Awake()
    {
        if (weaponHitbox != null)
        {
            damageHitbox = weaponHitbox.GetComponent<SkeletonDamageHitbox>();
            weaponHitbox.SetActive(false);
        }
    }

    public void EnableWeapon()
    {
        if (weaponHitbox == null || damageHitbox == null)
            return;

        if (weaponEnabled)
            return;

        weaponEnabled = true;

        damageHitbox.ResetSkeletonHitbox();
        weaponHitbox.SetActive(true);
    }

    public void DisableWeapon()
    {
        if (weaponHitbox == null)
            return;

        weaponEnabled = false;
        weaponHitbox.SetActive(false);
    }

    public void ForceDisableWeapon()
    {
        weaponEnabled = false;

        if (weaponHitbox != null)
            weaponHitbox.SetActive(false);
    }
}