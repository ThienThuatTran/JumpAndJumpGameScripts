using UnityEngine;

public class TrapFireButton : MonoBehaviour
{
    private Animator animator;
    private TrapFire trapFire;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        trapFire = GetComponentInParent<TrapFire>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            animator.SetTrigger("active");
            trapFire.SwitchOffFire();
        }
    }
}
