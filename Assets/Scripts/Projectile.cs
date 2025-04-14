using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;

    private bool canHitPlayer = true;
    private bool canHitenemy = true;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && canHitenemy)
        {
            canHitenemy = false;
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(20f);
                StartCoroutine(ResetHitEnemyFlag());
                return;
            }
            canHitenemy = true;
        }
        else if (other.CompareTag("Player") && canHitPlayer)
        {
            canHitPlayer = false;
            PlayerLife player = other.GetComponent<PlayerLife>();
            if (player != null)
            {
                player.TakeDamage(15f);
                StartCoroutine(ResetHitPlayerFlag());
                return;
            }
            canHitPlayer = true;
        }
        Destroy(gameObject);
    } 

    private IEnumerator ResetHitEnemyFlag()
    {
        yield return new WaitForSeconds(1f);
        canHitenemy = false;
    }

    private IEnumerator ResetHitPlayerFlag()
    {
        yield return new WaitForSeconds(2f);
        canHitPlayer = false;
    }
}