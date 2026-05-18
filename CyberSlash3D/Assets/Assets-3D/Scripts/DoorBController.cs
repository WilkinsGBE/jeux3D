using UnityEngine;

public class DoorBController : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool("OpenB", false); // force closed at start
    }

    public void OpenDoor()
    {
        animator.SetBool("OpenB", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("OpenB", false);
    }
}