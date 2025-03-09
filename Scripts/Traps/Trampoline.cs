using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float pushPower;
    [SerializeField] private float duration;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControllerHK player = collision.gameObject.GetComponent<PlayerControllerHK>();
        if (player != null)
        {
            animator.SetTrigger("activate");
            //player.Push(transform.up * pushPower, duration);
        }
    }
}
