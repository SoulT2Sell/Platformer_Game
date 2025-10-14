using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D[] colliders;
    protected SpriteRenderer sr;

    protected Transform player;
    [SerializeField] protected GameObject damageTrigger;
    [Space]

    [Header("Movment Options")]
    [SerializeField] protected float moveSpeed;
    protected bool isGrounded;
    protected bool isGroundDetected;
    protected bool isWallDetected;
    [SerializeField] protected float idleDuration;
    protected float idleTimer;
    protected bool canMove = true;

    [Header("Death Details")]
    [SerializeField] protected float deathImpact = 4f;
    [SerializeField] protected float rotationSpeed = 150f;
    protected bool isDead;
    protected int rotationDir = 1;

    [Header("Collision Infos")]
    [SerializeField] protected Transform groundTransform;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundDistance = 1.1f;
    [SerializeField] protected float wallDistance = .9f;
    [SerializeField] protected float playerDistance = 11f;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool playerDetected;

    protected int facingDir = -1;
    protected bool facingRight = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if(sr.flipX == true && !facingRight)
        {
            sr.flipX = false;
            Flip();
        }

        PlayerManager.OnPlayerRespawn += UpdatePlayerRefrence;
    }

    private void UpdatePlayerRefrence()
    {
        if (player == null)
            player = PlayerManager.instance.player.transform;
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;

        if (isDead)
            HandleRotation();
        HandleCollision();
    }

    public virtual void Die()
    {
        foreach (Collider2D collider in colliders)
            collider.enabled = false;
        damageTrigger.SetActive(false);
        anim.SetTrigger("isDead");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, deathImpact);
        isDead = true;

        if (Random.Range(0, 100) < 50)
            rotationDir = rotationDir * -1;

        PlayerManager.OnPlayerRespawn -= UpdatePlayerRefrence;
        Destroy(gameObject, 10);
    }
    protected virtual void HandleRotation()
    {
        transform.Rotate(0, 0, (rotationSpeed * rotationDir) * Time.deltaTime);
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
        isGroundDetected = Physics2D.Raycast(groundTransform.position, Vector2.down, groundDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallDistance, whatIsGround);
        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDistance, whatIsPlayer);
    }

    protected virtual void HandleFlip(float xValue)
    {
        if (isGrounded == false)
            return;

        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    [ContextMenu("Change facing diraction")]
    public void FlipDefultFacingDir()
    {
        sr.flipX = !sr.flipX;
    }

    private IEnumerator FlipCoroutine()
    {
        yield return new WaitForSeconds(idleDuration);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
        Gizmos.DrawLine(groundTransform.position, new Vector2(groundTransform.position.x, groundTransform.position.y - groundDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallDistance * facingDir), transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDistance * facingDir), transform.position.y));
    }
}
