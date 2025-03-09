using UnityEngine;

public class Mushroom : Enemy
{
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

}
