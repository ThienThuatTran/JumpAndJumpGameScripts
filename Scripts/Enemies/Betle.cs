using UnityEngine;

public class Betle : Enemy
{
    [SerializeField] private float gravityAfterDeath = 5f;
    [SerializeField] private bool isfacingLeft = true;
    private float lastXPosition;
    protected override void Update()
    {
        //
        if (isDead) return;
        HandleBetleFlip();
    }
    public override void Die()
    {
        GetComponent<MovingObject>().enabled = false;
        
        base.Die();
        rb.linearVelocityX = deathImpact;
        rb.gravityScale = gravityAfterDeath;
    }

    private void HandleBetleFlip()
    {
        if (lastXPosition < transform.position.x && !spriteRenderer.flipX)
        {
            //isfacingLeft =!isfacingLeft;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        else if (lastXPosition > transform.position.x && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        lastXPosition = transform.position.x;
        /*
        if (Mathf.Abs( rb.linearVelocityX) > Mathf.Epsilon)
        {
            
        }*/

        
    }
}
