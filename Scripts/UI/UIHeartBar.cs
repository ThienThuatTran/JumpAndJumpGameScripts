using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartBar : MonoBehaviour
{
    [SerializeField] private GameObject heartIcon;
    private int maxHealth;
    private int health;
    private HealthSystem playerHealth;
    private void Start()
    {
        playerHealth = PlayerStatus.Instance.GetPlayerHealth();
        maxHealth = playerHealth.GetMaxHealth();
        for (int i = 0; i < maxHealth; i++)
        {
            Instantiate(heartIcon, transform);
        }
        playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged; ;
    }

    private void PlayerHealth_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHeartBar();
    }

    private void UpdateHeartBar()
    {
        health = playerHealth.GetHealth();
        for (int i = health; i < maxHealth; i++)
        {
            Color fadeColor = transform.GetChild(i).GetComponent<Image>().color;
            fadeColor.a =.2f;
            transform.GetChild(i).GetComponent<Image>().color = fadeColor;
        }
    }
}
