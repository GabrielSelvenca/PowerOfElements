using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 60;
    public float currentHealth;

    public EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(Mathf.RoundToInt(maxHealth));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(Mathf.RoundToInt(currentHealth));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}