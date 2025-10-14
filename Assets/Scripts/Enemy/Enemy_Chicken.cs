using UnityEngine;

public class Enemy_Chicken : Enemy
{
    private BoxCollider2D cd;

    [Header("Movment info")]
    [SerializeField] private float aggroDuration;
    private float aggroTimer;
    private bool canFlip = true;

    protected override void Awake()
    {
        base.Awake();
        cd = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        aggroTimer -= Time.deltaTime;

        if (isDead)
            return;

        if (playerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;
        }

        if(aggroTimer <0)
            canMove = false;

        
        HandleMovment();

        if (isGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundDetected || isWallDetected)
        {
            Flip();
            canMove = false;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void HandleMovment()
    {
        if (!canMove)
            return;

        HandleFlip(player.position.x);
        
        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            if(canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), .3f);
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

}
