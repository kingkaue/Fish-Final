using System.Collections;
using UnityEngine;

public class EnemyShootScript : MonoBehaviour
{
    public Transform firePoint;
    private Transform player;
    public float detectionRange = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    private float nextFireTime = 0f;
    public GameObject projectilePrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {

        if (projectilePrefab == null || firePoint == null || player == null) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * projectileSpeed;

            projectile.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
        }
    }
}