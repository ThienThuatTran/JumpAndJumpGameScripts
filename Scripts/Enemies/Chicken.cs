using UnityEngine;

public class Chicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] private float aggroDuration;
    private float aggroTimer;
    private bool canFlip = true;

    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            return;
        }
        aggroTimer -= Time.deltaTime;
        
        
        if (isPlayerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;
        }
        if (aggroTimer < 0)
        {
            canMove = false;
        }
        HandleMovement();
        if (isGrounded)
        {
            HandleTurnAround();
        }

    }
    public override void Die()
    {
        base.Die();
    }

    private void HandleTurnAround()
    {
        if (!isGroundInFrontDetected || isWallDetected)
        {
            Flip();
            canMove = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleMovement()
    {
        if (!canMove)
        {
            return;
        }
        if (playerTransform == null) return;
        HandleFlip(playerTransform.position.x);
        if (isGrounded)
        {
            rb.linearVelocityX = facingDir * moveSpeed;
        }

    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && isFacingRight || xValue > transform.position.x && !isFacingRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), 0.3f);
            }
        }
    
    }
    protected override void Flip()
    {
        base.Flip();
        canFlip = false;
    }

}
