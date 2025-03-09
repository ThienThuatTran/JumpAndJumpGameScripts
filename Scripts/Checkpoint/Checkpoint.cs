using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool active = false;
    [SerializeField] private bool canBeReactivate = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( active && !canBeReactivate)
        {
            return;
        }
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            
            if (!active && !canBeReactivate) { AllowReactivate(); }
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        active = true;
        anim.SetTrigger("activate");
        GameManager.Instance.UpdateRespawnponit(transform);
    }

    private void AllowReactivate()
    {
        canBeReactivate = true;
    }

}
