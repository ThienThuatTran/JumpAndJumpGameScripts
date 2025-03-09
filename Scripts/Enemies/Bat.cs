using Pathfinding;
using UnityEngine;

public class Bat : Enemy
{
    [Header("Bat details")]
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AIDestinationSetter destinationSetter;

    [SerializeField] private float hangDuration = 1f;
    private float hangTimer = 0;

    [SerializeField] private float suspectDuration = 2f;
    private float suspectTimer;

    private Vector3 startPosition;

    private float attackDuration = 3f;
    private float attackTimer = 0;
    private enum State
    {
        Hang,
        Attack,
        Suspect,
        Recover

    }
    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        state = State.Hang;

    }
    private State state;
    private float lastXPosition;
    protected override void Update()
    {
        HandleBatFlip();
        switch (state)
        {
            case State.Hang:
                CheckPlayer();
                Hang();

                break;
            case State.Suspect:
                suspectTimer += Time.deltaTime;
                Suspect();
                break;
            case State.Recover:
                
                Recover();
                break;
            case State.Attack:
                Attack();
                break;

        }
    }

    private void HandleBatFlip()
    {
        if (transform.position.x > lastXPosition && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < lastXPosition && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        lastXPosition = transform.position.x;
    }
    public override void Die()
    {
        base.Die();
        rb.gravityScale = 5f;
        aiPath.enabled = false;
    }

    private void CheckPlayer()
    {
        if (playerTransform == null) return;
        aiPath.enabled = false;
        if (Vector2.Distance((Vector2)transform.position, (Vector2)playerTransform.position) <= playerDetectionRange)
        {
            hangTimer += Time.deltaTime;
        }
        else
        {
            hangTimer = 0;
        }
    }

    private void Hang()
    {
        if (hangTimer > hangDuration)
        {
            hangTimer = 0;
            animator.SetBool("isFlying", true);
            state = State.Attack;
            aiPath.enabled = true;
            destinationSetter.target.transform.position = playerTransform.position;
        }
    }

    private void Recover()
    {
        
        
        if (aiPath.reachedDestination)
        {
            Debug.Log("nghi");
            animator.SetBool("isFlying", false);
            state = State.Hang;
        }
        
        //CheckPlayer();
        //Hang();

    }

    private void Suspect()
    {
        
        if (suspectTimer > suspectDuration)
        {
            aiPath.enabled = true;
            suspectTimer = 0;
            state = State.Recover;
            destinationSetter.target.transform.position = startPosition;
        }
        
    }

    private void Attack()
    {
        destinationSetter.target.transform.position = playerTransform.position;
        if (Vector2.Distance(transform.position, playerTransform.position) > playerDetectionRange)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0;
        }

        if (attackTimer > attackDuration)
        {
            aiPath.enabled = false;
            state = State.Suspect;
        }

    }
}
