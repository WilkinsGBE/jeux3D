using UnityEngine;

public class SkeletonDeathManager : MonoBehaviour
{
    public SkeletonHealth[] skeletons;

    public void KillAllSkeletons()
    {
        foreach (SkeletonHealth skeleton in skeletons)
        {
            if (skeleton != null)
                skeleton.PermanentDeath();
        }
    }
}