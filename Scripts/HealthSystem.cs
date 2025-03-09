using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;
    private int maxHealth;
    private int health;


    public HealthSystem(int healthMax)
    {
        this.maxHealth = healthMax;
        health = healthMax;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return (float)health / maxHealth;
    }

    public void Damage(int damageAmount)
    {
        ScreenShake.Instance.DamageScreenShake();
        
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
            health = 0;
        }
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmout)
    {
        health += healAmout;
        if (health > maxHealth) health = maxHealth;
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

   

}
