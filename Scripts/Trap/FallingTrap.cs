using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private LayerMask playerLayeMask;
    private float startingGravity = 0f;
    [SerializeField] private float distance = 20f;
    [SerializeField] private float gravityScaleWhenFall = 10f;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = startingGravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasPlayer())
        {
            rigidbody2D.gravityScale = gravityScaleWhenFall;
        }
    }

    private bool HasPlayer()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, distance, playerLayeMask);
       
        Debug.DrawRay(transform.position, Vector2.down + new Vector2(0, -distance), Color.red);
        return raycastHit2D.collider != null;
    }
}
