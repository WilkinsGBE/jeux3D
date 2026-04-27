using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public Vector3 openRotation = new Vector3(0f, 90f, 0f);
 public AudioSource audioSource;   
    public AudioClip openSound;
    private bool isOpened = false;

    public void OpenDoor()
    {
        if (door == null)
        {
            Debug.LogError(" Door non assignée !");
            return;
        }

        if (isOpened) return;

        door.Rotate(openRotation);
          if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        isOpened = true;

        Debug.Log("🚪 Porte ouverte !");
    }
}
