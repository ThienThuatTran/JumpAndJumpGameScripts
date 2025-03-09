using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private HealthSystem playerHealth;
    private int maxHealth = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = new HealthSystem(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
