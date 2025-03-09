using UnityEngine;

public class Trunk : Enemy
{
    [Header("Trunk details")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 7f;
    private float lastTimeAttacked;
    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            return;
        }
        HandleMovement();

        if (isGrounded)
        {
            HandleTurnAround();
        }

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;
        if (canAttack && isPlayerDetected)
        {
            Attack();
        }

    }
    private void HandleTurnAround()
    {
        if (!isGroundInFrontDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
        {
            return;
        }
        if (isGrounded)
        {
            rb.linearVelocityX = facingDir * moveSpeed;
        }

    }
    private void Attack()
    {
        idleTimer = idleDuration + attackCooldown;
        lastTimeAttacked = Time.time;
        animator.SetTrigger("attack");
    }
    private void CreateBullet()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        bullet.SetXVelocity(bulletSpeed * facingDir);

        if (facingDir == 1)
        {
            bullet.FlipSprite();
        }

        Destroy(bulletGameObject, 10f);
    }
}
