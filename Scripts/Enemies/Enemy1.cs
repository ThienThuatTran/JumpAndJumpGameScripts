using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    [SerializeField] LayerMask platformLayerMask;
    [SerializeField] int towardRight = 1;
    [SerializeField] float enemyVelocity = 5;
    private bool canFlip = true;
    // Start is called before the first frame update
    void Start()
    {

        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //Debug.Log(boxCollider2D.bounds.max + boxCollider2D.bounds.center);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        bool isWAll = CheckWall();
        bool isDeep = CheckFrontGround();
        if (isWAll) { Flip(); }
        else if (!isDeep) { Flip(); }

        rigidbody2D.linearVelocity = new Vector2(towardRight *enemyVelocity, rigidbody2D.linearVelocity.y);
    }
    private void Flip()
    {
        //if (!canFlip) { return; }
        towardRight = -towardRight;
        transform.localScale = new Vector3(towardRight, 1, 1);
        canFlip = false;
        Invoke(nameof(this.AllowFlip), 0.2f);
    }

    private void AllowFlip()
    {
        canFlip = true;
    }
    private bool CheckFrontGround()
    {
        Vector2 boxPosition = new Vector2((boxCollider2D.bounds.extents.x*2 * towardRight + boxCollider2D.bounds.center.x) , boxCollider2D.bounds.center.y) ;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxPosition, 
            boxCollider2D.bounds.size, 0f, Vector2.down, 0.4f, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else rayColor = Color.red;

        Debug.DrawRay(boxPosition,
            Vector2.down *(0.2f +boxCollider2D.bounds.extents.y), rayColor);
        Debug.DrawRay(boxPosition,
    Vector2.down * (0.2f + boxCollider2D.bounds.extents.y), rayColor);
        return raycastHit.collider != null;
    }

    private bool CheckWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,towardRight * Vector2.right, 0.2f, platformLayerMask);
        return raycastHit.collider != null;
    }
}
