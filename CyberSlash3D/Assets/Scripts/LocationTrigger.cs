using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string locationName = "Ruines de la chapelle";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (LocationUI.instance != null)
                LocationUI.instance.ShowLocation(locationName);
        }
    }
}