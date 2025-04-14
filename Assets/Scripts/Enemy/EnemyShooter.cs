using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public float attackRange = 15f;
    public float fireRate = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;
    public int maxHealth = 60;

    private Transform player;
    private float fireCooldown;
    private int currentHealth;
    private EnemyHealthBar healthBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;
    }

    void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;
        }

        Destroy(projectile, 5f);
    }
}