using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    //Making the player take damage on each collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameObject tmp = Instantiate(deathParticle);
            tmp.transform.position = transform.position;
        }
    }
}
