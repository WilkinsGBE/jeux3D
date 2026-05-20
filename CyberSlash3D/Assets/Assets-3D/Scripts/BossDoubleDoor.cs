using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    public DoorAController doorA;
    public DoorBController doorB;

    [Header("Lock Interface")]
    public GameObject lockUI;
    public GameObject key3D;

    private bool playerNearby = false;
    private bool doorsOpen = false;
    private bool locked = false;
    private bool uiOpen = false;

    void Start()
    {
        if (lockUI != null)
            lockUI.SetActive(false);

        if (key3D != null)
            key3D.SetActive(false);
    }

    void Update()
    {
        if (locked) return;

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (KeyCollectible.keysCollected >= 2)
            {
                if (!uiOpen)
                {
                    OpenLockUI();
                }
            }
            else
            {
                Debug.Log("You need 2 keys to open this door!");
            }
        }
    }

    private void OpenLockUI()
    {
        uiOpen = true;

        if (lockUI != null)
            lockUI.SetActive(true);

        if (key3D != null)
            key3D.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void CloseLockUI()
    {
        uiOpen = false;

        if (lockUI != null)
            lockUI.SetActive(false);

        if (key3D != null)
            key3D.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TryOpenBossDoorFromUI()
    {
        if (KeyCollectible.keysCollected >= 2)
        {
            doorsOpen = true;
            SetDoors(true);
            CloseLockUI();
        }
        else
        {
            Debug.Log("You need 2 keys to open this door!");
        }
    }

    public void LockDoorsClosed()
    {
        locked = true;
        doorsOpen = false;
        CloseLockUI();
        SetDoors(false);
    }

    public void UnlockAndOpenDoors()
    {
        locked = false;
        doorsOpen = true;
        CloseLockUI();
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
        {
            playerNearby = false;
            CloseLockUI();
        }
    }
}