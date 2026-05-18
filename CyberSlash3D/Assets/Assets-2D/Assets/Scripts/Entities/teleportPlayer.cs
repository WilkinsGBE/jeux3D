using UnityEngine;
using UnityEngine.Audio;

public class teleportPlayer : MonoBehaviour
{
    private float teleportDistance =5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    public LayerMask groundLayer;
    public Vector3 boxCastOffset;
    public Vector2 boxSize;
    public float castDistance;

    public float tpCost;

    public movePlayer movePlayer;

    private bool isTeleporting;


    public AudioSource audioSource;
    public AudioClip teleportSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isTeleporting) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            audioSource.PlayOneShot(teleportSound);
            if (movePlayer == null) return;
            if (movePlayer.Stamina < tpCost) return;
            if (!canTeleport()) return;

            isTeleporting = true;

            anim.SetBool("isTeleportingEnter", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            movePlayer.Stamina -= tpCost;
            if (movePlayer.Stamina < 0f) movePlayer.Stamina = 0f;

            if (movePlayer.Stam_Fill != null && movePlayer.MaxStamina > 0f)
                movePlayer.Stam_Fill.fillAmount = movePlayer.Stamina / movePlayer.MaxStamina;

            if (movePlayer.recharge != null)
                movePlayer.StopCoroutine(movePlayer.recharge);

            movePlayer.recharge = movePlayer.StartCoroutine(movePlayer.RechargeStamina());
        }
    }
    public void Teleport()
    {
        float direction = sr.flipX ? -1f : 1f;

        if (canTeleport())
        {
            anim.SetBool("isTeleportingExit", true);
            rb.position += new Vector2(teleportDistance * direction, 0f);
        }
        else
        {
            endTeleport();
        }
    }

    public bool canTeleport()
    {
        float direction = sr.flipX ? -1f : 1f;
        Vector3 offset = boxCastOffset;
        offset.x = teleportDistance * direction;

        return !Physics2D.BoxCast(transform.position + offset, boxSize, 0, Vector2.down, castDistance, groundLayer);
    }
    public void endTeleport()
    {
        anim.SetBool("isTeleportingExit", false);
        anim.SetBool("isTeleportingEnter", false);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.1f);

        isTeleporting = false;
    }

    private void OnDrawGizmos()
    {
        sr = GetComponent<SpriteRenderer>();
        float direction = sr.flipX ? -1f : 1f;
        Vector3 offset = boxCastOffset;
        offset.x = teleportDistance * direction;

        Gizmos.DrawWireCube(transform.position + offset - transform.up * castDistance, boxSize);
    }

}
