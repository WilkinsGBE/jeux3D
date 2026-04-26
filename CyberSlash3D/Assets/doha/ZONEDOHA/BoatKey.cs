using UnityEngine;

public class BoatKey : MonoBehaviour
{
     public ObjectiveManager objectiveManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectiveManager.GetBoatKey();
            Destroy(gameObject);
        }
    }
}
