using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        if(isDead)
            return;

        HandleMovment();
        if (isGrounded)
             HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void HandleMovment()
    { 
        if(idleTimer > 0) 
            return;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }
}
