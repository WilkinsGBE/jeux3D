using UnityEngine;

public class BoatKey : MonoBehaviour
{
/*public ObjectiveManager objectiveManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            objectiveManager.GetBoatKey();
            Destroy(gameObject);
        }
    }*/
    public Zone3GameManager gameManager;

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        gameManager.CollectBoatKey();
        Destroy(gameObject);
    }
}
}
