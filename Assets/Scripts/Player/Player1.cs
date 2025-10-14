using System.Collections;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private GameObject playerFruit;
    [SerializeField] private DifficultyType gameDifficulty;
    
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cd;
    private GameManager gameManager;

    [Header("Movement ")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;
    private float defaultGravityScale;
    private bool canBeControled = false;

    [Header("Buffer & Coyote Jump")]
    [SerializeField] private float bufferJumpWindow = .25f;
    private float bufferJumpActivated = -1;
    [SerializeField] private float coyoteJumpWindow = .5f;
    private float coyoteJumpActivated = -1; 

    [Header("Wall interactions")]
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;

    [Header("knockback")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockBackPower;
    private bool isKnocked;

    [Header("Collision")]
    [SerializeField] private float groundDistanceCheck;
    [SerializeField] private float wallDistanceCheck;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isWallDetected;
    private bool isAirborne;
    [Space]
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Animation Details")]
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private ParticleSystem dustFX;
    [SerializeField] private int skinID; 

    private float xInput;
    private float yInput;   

    private bool facingRight = true;
    private int facingDir = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        defaultGravityScale = rb.gravityScale;
        UpdateGameDifficulty();
        RespawnFinished(false);
        UpdateSkin();
    }

    
    // Update is called once per frame
    private void Update()
    {
        UpdateAirborneStatus();

        if (canBeControled == false)
        {
            HandleCollision();
            HandleAnimations();
            return;
        }

        if (isKnocked)
            return;
        HandleEnemyDetection();
        HandleInput();
        HandleWallSlide();
        HandleMovment();
        HandleFlip();
        HandleCollision();
        HandleAnimations();

    }

    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Medium)
        {
            if (gameManager.GetCollectedFruits() <= 0)
            {
                Die();
                gameManager.RestartLevel();
            }
            else
            {
                ObjectCreator.instance.CreatObject(playerFruit, transform, 0, true);
                gameManager.RediusFruit();
            }
        }
        if(gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }
    }

    private void UpdateGameDifficulty()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager != null)
            gameDifficulty = difficultyManager.difficulty; 

    }

    public void UpdateSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        if (skinManager == null)
            return;

        anim.runtimeAnimatorController = animators[skinManager.getSkinID()];
    }
    private void HandleEnemyDetection()
    {
        if (rb.linearVelocity.y > 0)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyRadius, whatIsEnemy);
        foreach (var enemy in colliders)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();   
            if(newEnemy != null)
            {
                AudioManager.instance.PlaySFX(1);
                newEnemy.Die();
                Jump();
            }
        }
    }

    public void RespawnFinished(bool finished)
    {
        if (finished)
        {
            AudioManager.instance.PlaySFX(11);
            rb.gravityScale = defaultGravityScale;
            canBeControled = true;
            cd.enabled = true;
        }
        else
        {
            rb.gravityScale = 0;
            canBeControled = false;
            cd.enabled = false;
        }
    }

    public void Knockback(float sourceDamageXPosition)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceDamageXPosition)
            knockbackDir = -1;

        if (isKnocked)
            return ;

        AudioManager.instance.PlaySFX(9);
        CameraManager.instance.CameraShake(knockbackDir);
        StartCoroutine(KnockbackRoutine());
        rb.linearVelocity = new Vector2(knockBackPower.x * knockbackDir, knockBackPower.y);
    }

    private IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        anim.SetBool("IsKnocked", true);
        yield return new WaitForSeconds(knockBackDuration);
        anim.SetBool("IsKnocked", false);
        isKnocked = false;
    }

    public void Push(Vector2 forceDir, float duration = 0)
    {
        StartCoroutine(PushCoroutine(forceDir, duration));  
    }

    private IEnumerator PushCoroutine(Vector2 forceDir, float duration)
    {
        canBeControled = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(forceDir, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        canBeControled = true;
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(0);
        GameObject newFx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void UpdateAirborneStatus()
    {
        if (isGrounded && isAirborne)
            HandleLanding();
        if (!isGrounded && !isAirborne)
            BecomeAirborne();
    }

    private void BecomeAirborne()
    {
        isAirborne = true;
        if (rb.linearVelocity.y < 0)
        {
            ActivateCoyoteJump();
        }
    }

    private void HandleLanding()
    {
        dustFX.Play();
        isAirborne = false;
        canDoubleJump = true;
        AttemptBufferJump();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
            RequestBufferJump();
        } 
    }

    #region Buffer & Coyote Jump
    private void RequestBufferJump()
    {
        if (isAirborne)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;

    private void CancelCoyoteJump() => coyoteJumpActivated = Time.time - 1;
    #endregion

    private void JumpButton()
    {
        bool coyoteJumpAvalable = Time.time < coyoteJumpActivated + coyoteJumpWindow;
        if (isGrounded || coyoteJumpAvalable)
        {
            Jump();
        }
        else if(isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if(canDoubleJump)
        {
            DoubleJump();   
        }
        CancelCoyoteJump();
    }

    private void Jump()
    {
        dustFX.Play();
        AudioManager.instance.PlaySFX(3);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void WallJump()
    {
        dustFX.Play();
        AudioManager.instance.PlaySFX(12);
        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    private IEnumerator WallJumpRoutine() 
    { 
        isWallJumping = true;

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0;

        float yModifer = yInput < 0 ? 1 : .05f;

        if (canWallSlide == false)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifer);
    }
    
    private void DoubleJump()
    {
        dustFX.Play();
        AudioManager.instance.PlaySFX(3);
        isWallJumping = false;
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);  
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistanceCheck, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallDistanceCheck, whatIsGround);
    }

    private void HandleAnimations()
    {
        //anim.SetBool("IsRunning", rb.linearVelocity.x != 0);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsWalllDetected", isWallDetected);   
    }

    private void HandleMovment()
    {
        if (isWallDetected)
            return;

        if (isWallJumping)
            return;

        rb.linearVelocity = new Vector2 (xInput * speed, rb.linearVelocity.y);
    }

    private void HandleFlip()
    {
        if(xInput < 0 && facingRight || xInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyRadius);    
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , transform.position.y - groundDistanceCheck));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallDistanceCheck * facingDir), transform.position.y));
    }
}
