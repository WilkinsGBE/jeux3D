using UnityEngine;

public class ChurchTrigger : MonoBehaviour
{
    public BossChase boss;
    public BossDoorTrigger bossDoorTrigger;
    public BossHealth bossHealth;

    public SkeletonAI[] skeletons;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            bossDoorTrigger.LockDoorsClosed();
            boss.StartChasing();

            UIManager.instance.ShowBoss(bossHealth);

            foreach (SkeletonAI skeleton in skeletons)
            {
                if (skeleton != null)
                    skeleton.StartChasing();
            }
        }
    }
}