using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    public DoorAController doorA;
    public DoorBController doorB;

    private bool playerNearby = false;
    private bool doorsOpen = false;
    private bool locked = false;

    void Update()
    {
        if (locked) return;

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Check if player has at least 2 keys
            if (KeyCollectible.keysCollected >= 2)
            {
                doorsOpen = !doorsOpen;
                SetDoors(doorsOpen);
            }
            else
            {
                Debug.Log("You need 2 keys to open this door!");
            }
        }
    }

    public void LockDoorsClosed()
    {
        locked = true;
        doorsOpen = false;
        SetDoors(false);
    }

    public void UnlockAndOpenDoors()
    {
        locked = false;
        doorsOpen = true;
        SetDoors(true);
    }

    private void SetDoors(bool open)
    {
        if (open)
        {
            doorA.OpenDoor();
            doorB.OpenDoor();
        }
        else
        {
            doorA.CloseDoor();
            doorB.CloseDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}