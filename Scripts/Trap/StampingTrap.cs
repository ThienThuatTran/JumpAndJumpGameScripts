using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampingTrap : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private float stampingTime = 1f;
    [SerializeField] private float distance = 2.4f;
    private bool isStampingDown = false;
    private bool isStampingUp = false;
    [SerializeField] private float stampingDownSpeed = 40f;
    [SerializeField] private float stampingUpSpeed = 30f;
    private float startYPos = 0f;
    private float destinationYPos = 0f;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        startYPos = transform.position.y;
        destinationYPos = transform.position.y - distance;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(isStampingDown+ " " + isStampingUp);
        if (HasPlayer() && !isStampingDown && !isStampingUp)
        {
            Invoke(nameof(this.AllowStamping), stampingTime);
        }

        if (isStampingDown)
        {
            StampingDown();
        } 
        else if (isStampingUp)
        {
            StampingUp();
        }
    }

    private bool HasPlayer()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(transform.position, boxCollider2D.bounds.size, 0, Vector2.down, distance, playerLayerMask);
        return raycastHit2D.collider != null;
    }

    private void AllowStamping()
    {
        isStampingDown = true;
    }

    private void StampingDown()
    {
        if (transform.position.y > destinationYPos)
        {
            rigidbody2D.MovePosition(new Vector3(transform.position.x, transform.position.y - stampingDownSpeed * Time.deltaTime, transform.position.z));
        }
        else
        {
            transform.position = new Vector3(transform.position.x, destinationYPos, transform.position.z);
            isStampingDown = false;
            isStampingUp = true;
            Debug.Log(startYPos);
        }
    }
    private void StampingUp()
    {
        if (transform.position.y < startYPos)
        {
            rigidbody2D.MovePosition(new Vector3(transform.position.x, transform.position.y + stampingUpSpeed * Time.deltaTime, transform.position.z));
            Debug.Log("hihi");
        }
            
        else
        {
            transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
            isStampingUp = false;
        }
    }

}
