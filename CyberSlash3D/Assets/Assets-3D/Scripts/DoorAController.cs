using UnityEngine;

public class DoorAController : MonoBehaviour
{
    public Animator animator;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool("OpenA", false); // force closed at start
    }

    public void OpenDoor()
    {
        animator.SetBool("OpenA", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("OpenA", false);
    }
}