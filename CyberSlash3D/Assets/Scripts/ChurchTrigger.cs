using UnityEngine;

public class ChurchTrigger : MonoBehaviour
{
    public BossChase boss; // reference to your boss script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.StartChasing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boss.StopChasing();
        }
    }
}