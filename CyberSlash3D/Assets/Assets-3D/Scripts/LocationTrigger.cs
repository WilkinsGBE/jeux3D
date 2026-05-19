using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    string locationName =
    "Zone John: Ruines de la chapelle\n\n" +
    "Battez le Boss et récupérer la clé en sortant.\n\n" +
    "Attention: Le boss ressuscite les squelettes";

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