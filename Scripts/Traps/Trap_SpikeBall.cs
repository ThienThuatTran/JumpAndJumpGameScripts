using UnityEngine;

public class Trap_SpikeBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float pushPower;

    private void Start()
    {
        Vector2 pushVector = new Vector2(pushPower, 0);
        rb.AddForce(pushVector, ForceMode2D.Impulse);
    }
}
