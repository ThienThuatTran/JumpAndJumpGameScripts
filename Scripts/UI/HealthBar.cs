using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform redBar;
    private HealthSystem currentPlayerHealthSystem;

    public void Setup(HealthSystem playerHealthSystem)
    {
        currentPlayerHealthSystem = playerHealthSystem;
        redBar.localScale = Vector3.one;
        currentPlayerHealthSystem.OnHealthChanged += CurrentPlayerHealthSystem_OnHealthChanged;
    }

    private void CurrentPlayerHealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        redBar.localScale = new Vector3(currentPlayerHealthSystem.GetHealthPercent(), 1, 1);
    }



}
