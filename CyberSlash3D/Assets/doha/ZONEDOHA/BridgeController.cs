using UnityEngine;

public class BridgeController : MonoBehaviour
{

    public Transform door;
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);

    private bool isOpened = false;

    public void OpenDoor()
    {
        if (door == null)
        {
            Debug.LogError("❌ Door non assignée !");
            return;
        }

        if (isOpened) return;

        door.localRotation = Quaternion.Euler(openRotation);
        isOpened = true;

        Debug.Log("🚪 Porte ouverte !");
    }
}