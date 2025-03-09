using Pathfinding;
using UnityEngine;

public class Eagle : Enemy
{
    [Header("Ealge details")]
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private Vector3 downLeftPos;
    [SerializeField] private Vector3 upRightPos;
    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float chaseSpeed = 15f;
    [SerializeField] private float patrolSpeed = 5f;

    private Vector3 startPosition;
    [SerializeField] private float patrolDuration = 5f;
    private float patrolTimer = 0;

    [SerializeField] private float recoverDuration = 1f;
    private float recoverTimer = 0;

    private float attackDuration = 3f;
    private float attackTimer = 0;
    private enum State
    {
        Patrol,
        Attack,
        Recover

    }
    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        state = State.Patrol;

        PatrolRandomPos();
    }
    private State state;
    private float lastXPosition;
    protected override void Update()
    {
        
        switch (state)
        {
            case State.Patrol:
                patrolTimer += Time.deltaTime;
                CheckPlayer();
                Patrol();
                
                break;
            case State.Recover:
                recoverTimer += Time.deltaTime;
                Recover();
                break;
            case State.Attack:
                Attack();
                break;

        }
    }

    private void HandleEagleFlip()
    {
        if (transform.position.x > lastXPosition && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < lastXPosition && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        lastXPosition = transform.position.x;
    }
    public override void Die()
    {
        base.Die();
        rb.gravityScale = 5f;
        aiPath.enabled = false;
    }

    private void Patrol()
    {

        if (aiPath.reachedDestination || patrolTimer > patrolDuration)
        {
            aiPath.endReachedDistance = 1f;
            patrolTimer = 0;
            aiPath.enabled = false;
            state = State.Recover;
            //PatrolRandomPos();
        }
        

    }
    private void CheckPlayer()
    {
        if (playerTransform == null) return;
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, playerTransform.position -transform.position, playerDetectionRange, whatIsPlayer);

        RaycastHit2D hitObstacle = Physics2D.Raycast(transform.position, playerTransform.position -transform.position, playerDetectionRange, obstaclesLayerMask);

        if (hitPlayer.collider == null) return;
        
        float distanceToPlayer = hitPlayer.distance;
        float distanceToObstacle;
        if (hitObstacle.collider == null)
        {
            distanceToObstacle = float.MaxValue;
        }
        else
        {
            distanceToObstacle = hitObstacle.distance;
        }

        if (distanceToPlayer < distanceToObstacle)
        {
            aiPath.maxSpeed = chaseSpeed;
            state = State.Attack;
            destinationSetter.target.transform.position = playerTransform.position;
        }
    }

    private void PatrolRandomPos()
    {
        //bool validDestination = false;
        
        float xRandomPos = Random.Range(downLeftPos.x, upRightPos.x);
        float yRandomPos = Random.Range(downLeftPos.y, upRightPos.y);
        Vector3 patrolDestination = new Vector3(xRandomPos, yRandomPos, transform.position.z);
        aiPath.maxSpeed = patrolSpeed;
        destinationSetter.target.transform.position = patrolDestination;
    }

    private void Recover()
    {
        if (recoverTimer > recoverDuration)
        {
            recoverTimer = 0;
            animator.SetBool("isChasing", false);
            PatrolRandomPos();
            state = State.Patrol;
            aiPath.enabled = true;
        }
    }

    private void Attack()
    {
        animator.SetBool("isChasing", true);
        destinationSetter.target.transform.position = playerTransform.position;
        if (Vector3.Distance(transform.position, playerTransform.position) > playerDetectionRange)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0;
        }

        if (attackTimer > attackDuration)
        {
            state = State.Recover;
            aiPath.maxSpeed = patrolSpeed;
            //PatrolRandomPos();
        }
        
    }
}
