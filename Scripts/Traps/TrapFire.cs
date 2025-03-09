using System.Collections;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    [SerializeField] private float offDuration;

    private Animator animator;
    private CapsuleCollider2D fireCollider;
    [SerializeField] private TrapFireButton trapFireButton;
    private bool isActive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fireCollider = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        animator.SetBool("active", true);
        isActive = true;
    }
    public void SwitchOffFire()
    {
        if (!isActive)
        {
            return;
        }
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        SetFire(false);
        yield return  new WaitForSeconds(offDuration);
        SetFire(true);
    }

    private void SetFire(bool active)
    {
        animator.SetBool("active", active);
        fireCollider.enabled = active;
        isActive = active;
    }
}
