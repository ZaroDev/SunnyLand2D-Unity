using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyDamage : MonoBehaviour
{
    public static Action DoDamage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DoDamage?.Invoke();
        }
    }
}
