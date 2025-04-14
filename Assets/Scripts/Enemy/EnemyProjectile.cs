using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 20f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLife life = other.GetComponent<PlayerLife>();
            if (life != null)
            {
                life.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}