using UnityEngine;

public class Snail : Enemy
{
    public bool hasBody = true;
    [SerializeField] private GameObject snailBodyPrefab;
    [SerializeField] private float maxSpeed = 10f;
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
    public override void Die()
    {
        if (hasBody)
        {
            canMove = false;
            hasBody = false;
            animator.SetTrigger("hit");
            idleDuration = 0;
            rb.linearVelocity = Vector2.zero;
        }
        else if ( !canMove && !hasBody)
        {
            canMove=true;
            moveSpeed = maxSpeed;
            animator.SetTrigger("hit");
        }
        else
        {
            base.Die();
        }
        
    }
    private void CreateBody()
    {
        GameObject snailBodyGameObject = Instantiate(snailBodyPrefab, transform.position, Quaternion.identity);
        SnailBody snailBody = snailBodyGameObject.GetComponent<SnailBody>();

        if (Random.Range(0, 100) > 50)
        {
            deathRotationDirection = deathRotationDirection * -1;
        }

        snailBody.SetupBody(deathImpact, deathRotationDirection* deathRotationSpeed, facingDir);

        Destroy(snailBodyGameObject, 10);
    }
    private void HandleTurnAround()
    {
        bool canTurnAroundOnLedge = !isGroundInFrontDetected && hasBody;
        if (canTurnAroundOnLedge|| isWallDetected)
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
        if (!canMove)
        {
            return;
        }
        if (isGrounded)
        {
            rb.linearVelocityX = facingDir * moveSpeed;
        }

    }
    protected override void Flip()
    {
        base.Flip();

        if (hasBody)
        {
            animator.SetTrigger("wallHit");
        }
    }
}
