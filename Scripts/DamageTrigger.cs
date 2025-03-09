using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControllerHK playerController = collision.gameObject.GetComponent<PlayerControllerHK>();
        if (playerController != null)
        {
            //player.Damage();
            if (PlayerStatus.Instance.GetIsKnocked()) return;
            
            playerController.Knockback(transform.position.x);
        }
    }
    
}
