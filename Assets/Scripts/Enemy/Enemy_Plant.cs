using UnityEngine;

public class Enemy_Plant : Enemy
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
        if(isDead) 
            return;

        bool canAttack = Time.time > shotTime + shotCooldown;

        if (playerDetected && canAttack)
            Attack();
        
    }

    private void Attack()
    {
        shotTime = Time.time;
        anim.SetTrigger("Attack");
    }

    public void CreateBullet()
    {
        Bullet newbullet = Instantiate(bulletPrefab, bulletpoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(bulletSpeed * facingDir, 0);
        newbullet.HandleMovement(bulletVelocity);
        Destroy(newbullet.gameObject, 10);
    }
}
