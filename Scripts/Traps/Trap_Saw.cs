using System.Collections;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private Transform[] waypoints;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int waypointIndex = 0;
    private int moveDirection = 1;
    private bool canMove = true;

    private void Awake()
    {
        animator = GetComponent<Animator> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }
    private void Start()
    {
        canMove = true;
        transform.position = waypoints[waypointIndex].position;
        waypointIndex++;
    }

    private void Update()
    {
        animator.SetBool("active", canMove);
        if (!canMove)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.2f)
        {
            if (waypointIndex == 0 || waypointIndex == waypoints.Length - 1)
            {
                moveDirection *= -1;
                StartCoroutine(StopMovement(cooldown));
            }
            waypointIndex += moveDirection;
            
        }
    }

    private IEnumerator StopMovement(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
