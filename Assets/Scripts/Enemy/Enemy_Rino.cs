using Unity.Cinemachine;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino Details")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate;
    private float defaultSpeed;    
    [SerializeField] private Vector2 impactPower;

    [Header("Effects")]
    [SerializeField] private ParticleSystem dustFX;
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private Vector2 cameraImpulseDir;

    protected override void Start()
    {
        base.Start();
        defaultSpeed = moveSpeed;
        canMove = false;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void Update()
    {
        base.Update();

        if(isDead)
            return;

        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        if(playerDetected && isGrounded) 
            canMove = true;

        HandleCharge();
    }

    private void HitWallImpact()
    {
        dustFX.Play();
        impulseSource.DefaultVelocity = new Vector2(cameraImpulseDir.x * facingDir, cameraImpulseDir.y);
        impulseSource.GenerateImpulse();
    }

    private void HandleCharge()
    {
        if (canMove == false)
            return;
        moveSpeed = moveSpeed + (Time.deltaTime * speedUpRate);

        if (moveSpeed >= maxSpeed )
            maxSpeed = moveSpeed;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);

        if (isWallDetected)
        {
            HitWall();
        }

        if (isGroundDetected == false)
        {
            TurnAround();
        }
    }

    private void TurnAround()
    {
        moveSpeed = defaultSpeed;
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        Flip();
    }

    private void HitWall()
    {
        canMove = false;

        HitWallImpact();
        SpeedReset();
        anim.SetBool("hitWall", true);
        rb.linearVelocity = new Vector2(impactPower.x * -facingDir, impactPower.y);
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
    }

    public void ChargeIsOver()
    {
        anim.SetBool("hitWall", false);
        Invoke(nameof(Flip), 1);
    }

}
