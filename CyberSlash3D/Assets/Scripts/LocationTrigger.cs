using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "Ruines de la chapelle";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip locationSound;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (LocationUI.instance != null)
            LocationUI.instance.ShowLocation(locationName);

        if (audioSource != null && locationSound != null)
        {
            audioSource.PlayOneShot(locationSound);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or Location Sound on LocationTrigger.");
        }
    }
}