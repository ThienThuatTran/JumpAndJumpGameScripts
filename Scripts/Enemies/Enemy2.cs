using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private enum State
    {
        Sleeping,
        ChaseTarget,
        Attacking
    }
    private State state;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private float enemySpeed = 1f;
    private BoxCollider2D enemyCollider2D;
    private Rigidbody2D enemyRb;
    private Vector3 exploredPosition;
    [SerializeField] private float chaseTime = 5f;
    private float chaseTimer = -1;
    private float attackRange = 1.5f;
    private int towardRight = 0;
    [SerializeField] int lastTowardRight = -1;
    private float targetRange = 20f;
    private bool canFlip = false;

    private void Awake()
    {
        state = State.Sleeping;
        enemyCollider2D = GetComponent<BoxCollider2D>();
        enemyRb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        transform.localScale = transform.localScale = new Vector3(lastTowardRight * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        towardRight = lastTowardRight;
    }
    private void Update()
    {
        CheckFlip();
    }
    private void CheckFlip()
    {
        if (!canFlip) return;
        towardRight = SearchPlayerDirect();
        if (towardRight != 0)
        {
            transform.localScale = new Vector3(towardRight * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    

    void FixedUpdate()
    {

        bool hasPlayer;
        if (towardRight == 0) { hasPlayer = false; }
        else { hasPlayer = true; }

        Vector3 currentPlayerPosition = Player.Instance.GetPosition();

        float distanceFromPlayer = Vector3.Distance(transform.position, currentPlayerPosition);

        //Debug.Log(state + " " + distanceFromPlayer);
        switch (state)
        {
            default:
                break;
            case State.Sleeping:
                canFlip = false;
                enemyRb.linearVelocity = new Vector2(0, enemyRb.linearVelocity.y);
                if (HasPlayerInFront(lastTowardRight))
                {
                    exploredPosition = Player.Instance.GetPosition();
                    state = State.ChaseTarget;
                    chaseTimer = chaseTime;
                }
                break;
            case State.ChaseTarget:
                canFlip = true;
                if (!hasPlayer) 
                {
                    enemyRb.linearVelocity = new Vector2(0, enemyRb.linearVelocity.y);
                    chaseTimer -=Time.deltaTime;
                    if (chaseTimer < 0) 
                    {
                        state = State.Sleeping;
                    }
                }
                else
                {
                    lastTowardRight = towardRight;
                    enemyRb.linearVelocity = new Vector2(towardRight * enemySpeed, enemyRb.linearVelocity.y);
                    if (distanceFromPlayer < attackRange)
                    {
                        state = State.Attacking;
                    }
                }

                break;

            case State.Attacking:
                enemyRb.linearVelocity = new Vector2(0, enemyRb.linearVelocity.y);
                Debug.Log("Attack");
                if (distanceFromPlayer > attackRange || !hasPlayer)
                {
                    state = State.ChaseTarget;
                } 
                break;
        }
    }

    private bool HasPlayerInFront(int lastToward)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(enemyCollider2D.bounds.center, Vector2.right * lastToward, targetRange, playerLayerMask);
        RaycastHit2D raycastHit2DPlatform = Physics2D.Raycast(enemyCollider2D.bounds.center, Vector2.right * lastToward, Mathf.Infinity, platformLayerMask);
        //Debug.Log(raycastHit2D.distance + " " + raycastHit2DPlatform.distance);
        if (raycastHit2DPlatform.distance > Mathf.Epsilon)
        {
            if (raycastHit2D.distance > raycastHit2DPlatform.distance) return false;
        }
        return raycastHit2D.collider != null;
    }
    private int SearchPlayerDirect()
    {

        int playerDirection;
        Vector2 originalPosition = new Vector2();
        originalPosition = enemyCollider2D.bounds.center;
        RaycastHit2D raycastHit2DLeft = Physics2D.Raycast(originalPosition, Vector2.left, targetRange, playerLayerMask);
        RaycastHit2D raycastHit2DRight = Physics2D.Raycast(originalPosition, Vector2.right, targetRange, playerLayerMask);

        RaycastHit2D raycastHit2DLeftPlatform = Physics2D.Raycast(originalPosition, Vector2.left, Mathf.Infinity, platformLayerMask);
        RaycastHit2D raycastHit2DRightPlatform = Physics2D.Raycast(originalPosition, Vector2.right, Mathf.Infinity, platformLayerMask);

        Color rayColor;
        if (raycastHit2DLeft.collider != null)
        {
            rayColor = Color.green;
        }
        else rayColor = Color.red;
        
        Debug.DrawRay(originalPosition, Vector2.left + new Vector2( -targetRange,0), rayColor);

        if (raycastHit2DLeft.collider != null)
        {
            playerDirection = -1;
        }
        else if (raycastHit2DRight.collider != null)
        {
            playerDirection = 1;
        }
        else
        {
            playerDirection = 0;
        }
        if (playerDirection != 0)
        {
            if (raycastHit2DLeftPlatform.distance > Mathf.Epsilon || raycastHit2DLeftPlatform.distance > Mathf.Epsilon)
            {
                if (raycastHit2DLeft.distance > raycastHit2DLeftPlatform.distance && playerDirection == -1) return 0;
                else if (raycastHit2DRight.distance > raycastHit2DRightPlatform.distance && playerDirection == 1) return 0;
            }
        }

        return playerDirection;
    }

        private bool CheckFrontGround()
    {
        Vector2 boxPosition = new Vector2((enemyCollider2D.bounds.extents.x*2 * towardRight + enemyCollider2D.bounds.center.x) , enemyCollider2D.bounds.center.y) ;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxPosition, 
            enemyCollider2D.bounds.size, 0f, Vector2.down, 0.4f, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else rayColor = Color.red;

        Debug.DrawRay(boxPosition,
            Vector2.down *(0.2f +enemyCollider2D.bounds.extents.y), rayColor);
        Debug.DrawRay(boxPosition,
    Vector2.down * (0.2f + enemyCollider2D.bounds.extents.y), rayColor);
        return raycastHit.collider != null;
    }

    private bool CheckWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(enemyCollider2D.bounds.center, enemyCollider2D.bounds.size, 0f,towardRight * Vector2.right, 0.2f, platformLayerMask);
        return raycastHit.collider != null;
    }
}
