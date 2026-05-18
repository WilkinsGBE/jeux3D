using UnityEngine;

public class MoveEnnemy : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;

    public float speed;
    public Vector3 boxCastOffset;
    public Vector2 boxSize;
    public LayerMask groundLayer;
    public float castDistance;

    public Vector2 sideBoxSize;
    public Vector3 sideBoxOffset;
    public float sideCastDistance;

    [Header("Vision Cone")]
    public Transform player;
    public float visionDistance = 5f;
    [Range(0f, 180f)] public float visionAngle = 60f;
    public LayerMask GroundLayer;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 1.5f;
    public LayerMask playerLayer;
    public LayerMask visionBlockerLayer;

    private float lastAttackTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //anim.SetBool("isRunning", true);

    }

    // Update is called once per frame
    void Update()
    {

        // Vision
        if (CanSeePlayer())
        {
            Debug.Log("Player seen!");
            AttackPlayer();
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }



            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        if (!isGround() || IsWall())
        {
            speed *= -1;
            boxCastOffset.x = -boxCastOffset.x;
            sideBoxOffset.x = -sideBoxOffset.x;
            flip();
        }


    }
    public bool isGround()
    {
        return Physics2D.BoxCast(transform.position + boxCastOffset, boxSize, 0, -transform.up, castDistance, groundLayer);
    }

    public bool IsWall()
    {
        return Physics2D.BoxCast(transform.position + sideBoxOffset, sideBoxSize, 0, transform.right * Mathf.Sign(speed), sideCastDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + boxCastOffset - transform.up * castDistance, boxSize);
        Gizmos.DrawWireCube(transform.position + sideBoxOffset, sideBoxSize);

        Gizmos.DrawWireSphere(transform.position, visionDistance);

        Vector3 forward = Application.isPlaying
            ? (Mathf.Sign(speed) > 0 ? Vector3.right : Vector3.left)
            : Vector3.right;

        float halfAngle = visionAngle * 0.5f;

        Vector3 leftBoundary = Quaternion.Euler(0, 0, halfAngle) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -halfAngle) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * visionDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * visionDistance);
    }



    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }


    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 origin = transform.position;
        Vector2 toPlayer = (player.position - transform.position);
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > visionDistance)
            return false;

        Vector2 forward = Mathf.Sign(speed) > 0 ? Vector2.right : Vector2.left;
        float angleToPlayer = Vector2.Angle(forward, toPlayer.normalized);

        if (angleToPlayer > visionAngle * 0.5f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, visionDistance, groundLayer);

        if (hit.collider != null && hit.transform != player)
            return false;

        return true;
    }

    public void AttackPlayer()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        anim.SetBool("isAttacking", true);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            float direction = Mathf.Sign(transform.localScale.x);
            projScript.SetDirection(-Mathf.Sign(transform.localScale.x));
        }
    }
}
