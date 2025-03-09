using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 40;
    private HealthSystem enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = new HealthSystem(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectiles"))
        {
            enemyHealth.Damage(collision.gameObject.GetComponent<Arrow>().GetArrowDamage());
            if (enemyHealth.GetHealth() <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
