using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D[] colliders;
    private Animator animator;
    [SerializeField] private float speed = 0.75f;
    [SerializeField] private float travelDistance;
    [SerializeField] private float fallDelay;
    private Vector3[] waypoints;
    private int waypointIndex = 0;
    private bool canMove = false;

    [SerializeField] private float impactSpeed = 3;
    [SerializeField] private float impactDuration = 0.1f;
    private float impactTimer;
    private bool impactHappened = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliders = GetComponents<BoxCollider2D>();
    }
    private void Start()
    {
        SetupWaypoints();
        float randomDelay = Random.Range(0, 0.7f);
        Invoke(nameof(this.ActivePlatform), randomDelay);
    }
    private void Update()
    {
        HandleMovement();
        HandleImpact();
    }

    private void ActivePlatform()
    {
        canMove = true;
    }

    private void SetupWaypoints()
    {
        waypoints = new Vector3[2];
        float yOffset = travelDistance / 2;

        waypoints[0] = transform.position + new Vector3 (0, yOffset, 0);
        waypoints[1] = transform.position + new Vector3 (0, -yOffset, 0);
    }
    private void HandleMovement()
    {
        if (!canMove)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex], speed *Time.deltaTime);
        if (Vector2.Distance(transform.position, waypoints[waypointIndex]) < 0.1f)
        {
            waypointIndex++;
            if (waypointIndex > waypoints.Length - 1)
            {
                waypointIndex = 0;
            }
        }
    }
    private void HandleImpact()
    {
        if (impactTimer < 0)
        {
            return;
        }

        impactTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.down * 10f, impactSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactHappened)
        {
            return;
        }
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null) 
        {
            Invoke(nameof(this.SwitchOffFallingPlatform), fallDelay);
            impactTimer = impactDuration;
            impactHappened = true;
        }
    }

    private void SwitchOffFallingPlatform()
    {
        animator.SetTrigger("deactive");
        canMove = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3.5f;
        rb.linearDamping = 0.5f;

        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }

    }

}
