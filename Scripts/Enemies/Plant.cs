using UnityEngine;

public class Plant : Enemy
{
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float bulletSpeed = 7f;
    private float lastTimeAttacked;
    protected override void Update()
    {
        base.Update();

        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;
        if (canAttack && isPlayerDetected)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastTimeAttacked = Time.time;
        animator.SetTrigger("attack");
    }
    private void CreateBullet()
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();
        
        bullet.SetXVelocity(bulletSpeed * facingDir);

        Destroy(bulletGameObject, 10f);
    }
}
