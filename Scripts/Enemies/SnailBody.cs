using UnityEngine;

public class SnailBody : MonoBehaviour
{
    private Rigidbody2D rb;
    private float zRotation;
    private SpriteRenderer sr;

    public void SetupBody(float yVelocity, float zRotation, int facingDir)
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.linearVelocityY = yVelocity;
        this.zRotation = zRotation;
        if (facingDir == 1)
        {
            sr.flipX = true;
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, zRotation* Time.deltaTime);
    }
}
