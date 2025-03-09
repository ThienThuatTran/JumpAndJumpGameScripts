using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControllerHK playerController = collision.GetComponent<PlayerControllerHK>();
        if (playerController != null)
        {
            playerController.InDeadZone();
            //Destroy(playerController.gameObject);
            playerController.gameObject.SetActive(false);

        }
    }
}
