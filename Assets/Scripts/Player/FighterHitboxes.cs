using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class FighterHitboxes : MonoBehaviour
{
    public float coreDamage = 2;
    public GameObject knockbackenemy;
    public float knockbackForce = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                // Apply damage
                enemyStats._currentHealth = Mathf.Max(enemyStats._currentHealth - coreDamage, 0);
                enemyStats.HealthBar.UpdateHealthBar(enemyStats.MaxHealth, enemyStats._currentHealth);

                // Apply knockback
                Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    ApplyKnockBack(enemyRb, other.transform);
                }
            }
        }
    }

    void ApplyKnockBack(Rigidbody enemyRb, Transform enemyPosition)
    {
        Vector3 knockbackDirection = (enemyPosition.position - transform.position).normalized;
        knockbackDirection.y = 0; // Prevent enemy from flying upwards

        enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        // Increase drag to slow down the enemy over time
        StartCoroutine(ResetDrag(enemyRb, 0.5f)); // Adjust delay as needed
    }

    IEnumerator ResetDrag(Rigidbody enemyRb, float delay)
    {
        float originalDrag = enemyRb.linearDamping; // Store original drag value
        enemyRb.linearDamping = 5f; // Increase drag to make the enemy slow down

        yield return new WaitForSeconds(delay);

        enemyRb.linearDamping = originalDrag; // Reset to normal drag
    }

}
