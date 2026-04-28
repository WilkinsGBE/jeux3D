using UnityEngine;

public class BossFistHitboxController : MonoBehaviour
{
    public GameObject fistHitbox;
    private DamageHitbox damageHitbox;

    private void Awake()
    {
        damageHitbox = fistHitbox.GetComponent<DamageHitbox>();
        fistHitbox.SetActive(false);
    }

    public void EnableFist()
    {
        damageHitbox.ResetHitbox(); 
        fistHitbox.SetActive(true);
    }

    public void DisableFist()
    {
        fistHitbox.SetActive(false);
    }
}