using TMPro;
using UnityEditor;
using UnityEngine;

public class NewFallingPlatform : MonoBehaviour
{
    [SerializeField] private float speedByPlayerWeight = 2f;
    [SerializeField] private float speed = 2f;

    [SerializeField] private LayerMask playerLayerMask;
    private Vector3[] idlePositionArray;
    private Vector3 startPosition;
    private float playerDtRadius;
    private int idleIndex = 0;


    private BoxCollider2D boxCollider;
    private enum FallingPlatformState
    {
        isIdle,
        isFalling,
        isRecovering
    }

    private FallingPlatformState state;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.position;
    }
    private void Start()
    {
        state = FallingPlatformState.isIdle;
        idlePositionArray = new Vector3[2];
        float randomYOffset = Random.Range(0.5f, 1f);
        idlePositionArray[0] = new Vector3(transform.position.x, transform.position.y + randomYOffset);
        idlePositionArray[1] = new Vector3(transform.position.x, transform.position.y - randomYOffset);
        playerDtRadius = boxCollider.bounds.size.x *0.5f;
    }

    private void Update()
    {
        switch (state)
        {
            case FallingPlatformState.isIdle:
                PlatformIdleMove();
                if (IsTouchingPlayer())
                {
                    state = FallingPlatformState.isFalling;
                }
                break;
            case FallingPlatformState.isFalling:
                PlatformFallingMove();
                if (!IsTouchingPlayer())
                {
                    state = FallingPlatformState.isRecovering;
                }
                break;
            case FallingPlatformState.isRecovering:
                PlatformRecoveringMove();
                if (IsTouchingPlayer())
                {
                    state = FallingPlatformState.isFalling;
                }
                break;
        }



    }

    

    private void PlatformIdleMove()
    {
        if (Vector3.Distance(transform.position, idlePositionArray[idleIndex]) >0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, idlePositionArray[idleIndex], speed * Time.deltaTime);
        }
        else
        {
            idleIndex++;
            if (idleIndex == idlePositionArray.Length)
            {
                idleIndex = 0;
            }
        }
        
    }

    private void PlatformFallingMove()
    {
        transform.Translate(0, -speedByPlayerWeight * Time.deltaTime, 0);
    }

    private void PlatformRecoveringMove()
    {
        if ((Vector3.Distance(transform.position, startPosition) > 0.1f))
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speedByPlayerWeight * Time.deltaTime);
        }
        else
        {
            state = FallingPlatformState.isIdle;
        }
    }

    private bool IsTouchingPlayer()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(transform.position,  playerDtRadius +0.1f, playerLayerMask);
        return collider2D != null;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, playerDtRadius + 0.1f);
    }
}
