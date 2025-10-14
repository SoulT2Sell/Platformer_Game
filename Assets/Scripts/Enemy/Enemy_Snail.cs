using UnityEngine;

public class Enemy_Snail : Enemy
{
    [Header("Snail Details")]
    [SerializeField] private Enemy_SnailBody bodyPrefab;
    private bool hasBody = true;
    [SerializeField] private float maxSpeed = 10;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        if (isDead)
            return;

        HandleMovment();
        if (isGrounded)
            HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        bool canFlipFromLedge = !isGroundDetected && hasBody;
        if (canFlipFromLedge || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector3.zero;
        }
    }
    public override void Die()
    {
        if(hasBody)
        {
            canMove = false;
            hasBody = false;
            anim.SetTrigger("isDead");

            rb.linearVelocity = Vector2.zero;
            idleDuration = 0;
        }
        else if(canMove == false && hasBody == false)
        {
            anim.SetTrigger("isDead");
            canMove = true;
            moveSpeed = maxSpeed;
        }
        else
        {
            base.Die();
        }
    }
    private void HandleMovment()
    {
        if (idleTimer > 0)
            return;

        if (canMove == false) 
            return;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }

    protected override void Flip()
    {
        base.Flip();
        if (hasBody == false)
            anim.SetTrigger("wallHit");
    }

    public void CreatBody()
    {
        Enemy_SnailBody newBody = Instantiate (bodyPrefab, transform.position, Quaternion.identity);
        if (Random.Range(0, 100) < 50)
            rotationDir = rotationDir * -1;
        newBody.SetupBody(deathImpact, rotationSpeed * rotationDir, facingDir);
        Destroy (newBody.gameObject, 10);
    }
}
