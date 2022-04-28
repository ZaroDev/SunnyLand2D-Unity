using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerHealth : MonoBehaviour
{
    public IntVariable health;


    public static event Action OnDamage;

    private void OnEnable()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health.value -= amount;
        if(health.value <= 0)
        {
            health.value = 0;
        }

        OnDamage?.Invoke();
    }
}
