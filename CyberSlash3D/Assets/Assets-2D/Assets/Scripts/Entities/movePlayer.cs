using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class movePlayer : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameManager2D gameManager;

    public float speed = 10f;
    public float jumpForce = 12f;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public LayerMask enemies;
    public Vector3 boxCastOffset;
    //public Vector3 attackPointFlip;
    public GameObject attackPoint;
    public float radius;
    public float playerDmg;

    public Image Stam_Fill;
    public float Stamina, MaxStamina;
    public float attack2Cost;
    public float ChargeRate;

    private bool isAttacking;

    public Coroutine recharge;
    public bool hasAccessCard;
    public int coinAmount;

    // Audio
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip jumpSound;
    public float footstepInterval = 0.1f;
    private float footstepTimer = 0f;



    private bool doubleJump;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager2D>();
        audioSource = GetComponent<AudioSource>();
        playerDmg = 25;
    }

    void Update()
    {
        if (Stamina < 0) Stamina = 0f;
        #region:Movement
        float direction = UnityEngine.Input.GetAxis("Horizontal");


        // Horizontal movement
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (rb.linearVelocity.x != 0 && isGrounded())
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        // Flip sprite & attack point
        if (direction < 0)
        {
            sr.flipX = true;
            Vector3 pos = attackPoint.transform.localPosition;
            pos.x = -Mathf.Abs(pos.x);
            attackPoint.transform.localPosition = pos;
        }
        else if (direction > 0)
        {
            sr.flipX = false;
            Vector3 pos = attackPoint.transform.localPosition;
            pos.x = Mathf.Abs(pos.x);
            attackPoint.transform.localPosition = pos;
        }

        // Fast fall + Jump
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (!isGrounded())
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        // Double jumpe false if grounded

        //if (isGrounded() && !UnityEngine.Input.GetButton("Jump"))
        //{
        //    audioSource.PlayOneShot(jumpSound);
        //    doubleJump = false;
        //}

        // Jump
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded() || doubleJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                audioSource.PlayOneShot(jumpSound);
                doubleJump = !doubleJump;
            }
        }

        // Fast fall when pressing S
        if (UnityEngine.Input.GetKey(KeyCode.S) && !isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -20f);
        }

        // Falling
        if (!isGrounded() && rb.linearVelocity.y < 0)
        {
            anim.SetBool("isMoving", false);
        }
        #endregion

        // Attack 1
        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool("isAttacking", true);
            audioSource.PlayOneShot(attackSound);
        }

        // Attack 2
        if (isAttacking) return;
        if (UnityEngine.Input.GetKeyDown(KeyCode.K) && Stamina > 30f)
        {
            audioSource.PlayOneShot(attackSound);
            isAttacking = true;
            playerDmg = playerDmg + playerDmg * 2;
            anim.SetBool("isAttacking2", true);

            Stamina -= attack2Cost;
            if (Stamina < 0) Stamina = 0;
            Stam_Fill.fillAmount = Stamina / MaxStamina;

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }


    }

    #region:isGrounded Boxcast
    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position + boxCastOffset, boxSize, 0f, Vector2.down, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion



    public void attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemies);

        foreach (Collider2D hit in hits)
        {
            Enemyhealth enemyHealth = hit.GetComponent<Enemyhealth>();
            if (enemyHealth != null)
            {
                Debug.Log("Hit ground enemy");
                enemyHealth.health -= playerDmg;
                continue;
            }

            // doha enemies Flying demon
            FlyingDemon demon = hit.GetComponent<FlyingDemon>();
            if (demon != null)
            {
                Debug.Log("Hit flying demon");
                demon.TakeDamage((int)playerDmg);
            }
        }
    }

    public void endAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isAttacking2", false);
        playerDmg = 25;
        isAttacking = false;
    }

    public IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 10f;
            if (Stamina > MaxStamina) Stamina = MaxStamina;
            if (Stam_Fill != null && MaxStamina > 0f)
                Stam_Fill.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(.1f);
        }
        recharge = null;
    }

    #region:OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + boxCastOffset - transform.up * castDistance, boxSize);
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);

    }
    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OutOfBounds"))
        {
            if (gameManager != null)
            {
                gameManager.PlayerDied();
            }
            else
            {
                Debug.LogError("GameManager2D not found in scene!");
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        Vector3 normal = other.GetContact(0).normal;
    //        if (normal == Vector3.up)
    //        {
    //            isGrounded = true;
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(UnityEngine.Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
