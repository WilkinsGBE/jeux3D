using UnityEngine;

public class SkeletonChaseZone : MonoBehaviour
{
    public PatrolSkeletonAI[] skeletons;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (PatrolSkeletonAI skeleton in skeletons)
            {
                skeleton.StartChasing();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (PatrolSkeletonAI skeleton in skeletons)
            {
                skeleton.StopChasing();
            }
        }
    }
}