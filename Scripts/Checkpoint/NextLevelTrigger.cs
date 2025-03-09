using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            GameManager.Instance.LoadNextLevel();
            PlayerPrefs.SetInt(GameManager.Instance.GetCurrentLevelName() + "Tutorial" + "isCompleted", 1);
        }
        
    }
}
