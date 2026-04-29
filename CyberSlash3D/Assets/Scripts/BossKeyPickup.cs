using UnityEngine;

public class BossKeyPickup : MonoBehaviour
{
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

        gameObject.SetActive(false);
    }
}