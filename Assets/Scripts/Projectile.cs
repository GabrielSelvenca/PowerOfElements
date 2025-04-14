using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public enum ProjectileOwner
{
    Player,
    Enemy
}

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public ProjectileOwner owner;

    private bool canHitPlayer = true;
    private bool canHitenemy = true;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner == ProjectileOwner.Player && other.CompareTag("Enemy") && canHitenemy)
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
        else if (owner == ProjectileOwner.Enemy && other.CompareTag("Player") && canHitPlayer)
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
        canHitenemy = true;
    }

    private IEnumerator ResetHitPlayerFlag()
    {
        yield return new WaitForSeconds(2f);
        canHitPlayer = true;
    }
}