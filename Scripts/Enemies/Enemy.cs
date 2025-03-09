using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float idleDuration = 1f;
    [SerializeField] protected GameObject enemyDeathFX;
    protected float idleTimer = 0;
    protected SpriteRenderer spriteRenderer;

    [Header("Death details")]
    [SerializeField] protected float deathImpact;
    [SerializeField] protected float deathRotationSpeed;
    protected Transform playerTransform;

    [SerializeField] protected float playerDetectionRange;
    protected bool isPlayerDetected = false;

    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool canMove = true;
    [SerializeField] protected Collider2D[] colliders;
    protected int deathRotationDirection = 1;
    protected bool isDead = false;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected int facingDir = -1;
    protected bool isWallDetected;
    protected bool isGrounded;
    protected bool isGroundInFrontDetected;
    protected bool isFacingRight = false;
    
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
        
    }
    protected virtual void Start()
    {
        InvokeRepeating(nameof(UpdatePlayerRef), 0, 1);
        if (spriteRenderer.flipX && !isFacingRight)
        {
            spriteRenderer.flipX = false;
            Flip();
        }
    }

    private void UpdatePlayerRef()
    {
        
        if (Player.Instance == null) return;
        if (playerTransform == null)
        {
            playerTransform = Player.Instance.transform;
        }
        
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleAnimator();

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
        }

        if (isDead)
        {
            HandleDeath();
        }
    }
    protected virtual void HandleAnimator()
    {
        animator.SetFloat("xVelocity", rb.linearVelocityX);
    }
    public virtual void Die()
    {
        isDead = true;

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        FreeEnemyFromConstraints();
        
        EnemyDeathFX();

        GameManager.Instance.AddDestroyedEnemies();
        Destroy(gameObject, 10);
    }

    private void EnemyDeathFX()
    {
        animator.SetTrigger("hit");
        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);

        rb.linearVelocityY = deathImpact;
        if (Random.Range(0, 100) > 50)
        {
            deathRotationDirection = deathRotationDirection * -1;
        }
        
    }

    private void FreeEnemyFromConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void HandleDeath()
    { 

        transform.Rotate(0, 0, deathRotationSpeed * deathRotationDirection * Time.deltaTime);
    }
    protected virtual void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }

    [ContextMenu("Change Facing Direction")]
    public void FlipDefaultFacingDirection()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    protected virtual void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && isFacingRight || xValue > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }
    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);
        isGroundInFrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayerMask);
        isWallDetected = Physics2D.Raycast(transform.position , Vector2.right * facingDir, wallCheckDistance, groundLayerMask);
        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectionRange, whatIsPlayer);
        Debug.DrawRay(transform.position, Vector2.right * facingDir * wallCheckDistance);
    }
}
