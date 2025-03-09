using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] private GameObject itemCollectedFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {

            Instantiate(itemCollectedFX, transform.position, Quaternion.identity);
            GameManager.Instance.AddCollectedGems();
            SoundFXManager.Instance.PlayeSoundFX(SoundFXManager.Instance.collectItemSFX, transform, 1f);
            Destroy(gameObject, 0.05f);
        }
    }
}
