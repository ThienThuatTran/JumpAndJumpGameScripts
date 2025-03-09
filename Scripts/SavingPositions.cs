using UnityEngine;

public class SavingPositions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            
            GameManager.Instance.UpdateRespawnponit(transform);
        }
    }
}
