using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void FlipSprite()
    {
        sr.flipX = !sr.flipX;
    }
    public void SetXVelocity(float xVelocity)
    {
        rb.linearVelocityX = xVelocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            PlayerControllerHK playerController = collision.gameObject.GetComponent<PlayerControllerHK>();
            if (playerController == null)
            {
                return;
            }
            if (!PlayerStatus.Instance.GetIsKnocked())
            {
                playerController.Knockback(transform.position.x);
            }
            
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(gameObject);
        }
        

    }
}
