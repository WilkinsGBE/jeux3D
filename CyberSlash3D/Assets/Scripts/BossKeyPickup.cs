using UnityEngine;

public class BossKeyPickup : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip pickupSound;

    public InventoryManager inventory;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (inventory == null)
        {
            Debug.LogError("InventoryManager not assigned on BossKeyPickup!");
            return;
        }

        inventory.hasBossKey = true;
        Debug.Log("Boss key acquired! hasBossKey = " + inventory.hasBossKey);

       
        if (audioSource != null && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        // Hide object AFTER playing sound (with delay)
        Destroy(gameObject, pickupSound.length);
    }
}