using UnityEngine;

public class Rhino : Enemy
{
    [Header("Rhino details")]
    
    [SerializeField] private Vector2 impactPower;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUpRate = 0.6f;
    
    private float defaultSpeed;
    private void Start()
    {
        canMove = false;
    }
    protected override void Awake()
    {
        base.Awake();
        defaultSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();

        HandleCharge();
        HandleCollision();

    }
    private void HandleCharge()
    {
        if (!canMove)
        {
            return;
        }

        rb.linearVelocityX = moveSpeed * facingDir;
        HandleSpeedUp();

        if (isWallDetected)
        {
            WallHit();
        }
        if (!isGroundInFrontDetected)
        {
            TurnAround();
        }

    }

    private void HandleSpeedUp()
    {
        moveSpeed += speedUpRate * Time.deltaTime;
        if (moveSpeed >= maxSpeed)
        {
            moveSpeed = maxSpeed;
        }
    }

    private void TurnAround()
    {
        canMove = false;
        SpeedReset();
        rb.linearVelocity = Vector2.zero;
        Flip();
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
    }

    private void WallHit()
    {
        SpeedReset();
        animator.SetBool("hitWall", true);
        canMove = false;
        rb.linearVelocity = new Vector2(impactPower.x * -facingDir, impactPower.y);
    }

    private void ChargeIsOver()
    {
        animator.SetBool("hitWall", false);
        Invoke(nameof(Flip), 1f);
        
    }
    protected override void HandleCollision()
    {
        base.HandleCollision();


        
        if (isPlayerDetected && isGrounded) 
        {
            canMove = true;
        }
    }
}
