using UnityEngine;

public class Enemy_Trunk : Enemy
{
    [Header("BulletShotInfo")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletpoint;
    [SerializeField] private float bulletSpeed = 7;
    private float shotTime;
    [SerializeField] private float shotCooldown = 1.5f;
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

        bool canAttack = Time.time > shotTime + shotCooldown;

        if (playerDetected && canAttack)
            Attack();

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
        if (idleTimer > 0)
            return;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }
    private void Attack()
    {
        shotTime = Time.time;
        idleTimer = idleDuration + shotCooldown;
        anim.SetTrigger("Attack");
    }

    public void CreateBullet()
    {
        Bullet newbullet = Instantiate(bulletPrefab, bulletpoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(bulletSpeed * facingDir, 0);
        newbullet.HandleMovement(bulletVelocity);
        if(facingDir == 1)
            newbullet.Flip_Bullet();
        Destroy(newbullet.gameObject, 10);
    }
}
