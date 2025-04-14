using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 25f;

    private bool canShot = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && projectilePrefab != null)
        {
            if (!canShot) return;
            canShot = false;

            GameObject clone = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            clone.name = "projetilPlayer";

            Rigidbody rb = clone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }

            StartCoroutine(CanPlayerShot());

            Destroy(clone, 5f);
        }
    }

    private IEnumerator CanPlayerShot()
    {
        yield return new WaitForSeconds(1f);
        canShot = true;
    }
}