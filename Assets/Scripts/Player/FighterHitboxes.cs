using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class FighterHitboxes : MonoBehaviour
{
    public float coreDamage = 2;
    public GameObject knockbackenemy;
    public float knockbackForce = 10f;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                // Checks if player has crit augment
                if (player.GetComponent<CharacterClass_FIghter>().canCrit)
                {
                    // Chooses a random number between 1 and 4
                    int critRoll = Random.Range(1, 5);

                    // If random number is 4, do crit damage (double core damage)
                    if (critRoll == 4)
                    {
                        Debug.Log("Crit!");
                        enemyStats._currentHealth = Mathf.Max(enemyStats._currentHealth - (coreDamage * 2), 0);
                        enemyStats.HealthBar.UpdateHealthBar(enemyStats.MaxHealth, enemyStats._currentHealth);
                    }
                    else
                    {
                        enemyStats._currentHealth = Mathf.Max(enemyStats._currentHealth - coreDamage, 0);
                        enemyStats.HealthBar.UpdateHealthBar(enemyStats.MaxHealth, enemyStats._currentHealth);
                    }
                }

                // Applies damage regularly if no crit augment
                else
                {
                    // Apply damage
                    enemyStats._currentHealth = Mathf.Max(enemyStats._currentHealth - coreDamage, 0);
                    enemyStats.HealthBar.UpdateHealthBar(enemyStats.MaxHealth, enemyStats._currentHealth);
                }

                // Checks if player has vampiric strike augment
                if (player.GetComponent<CharacterClass_FIghter>().isVampiricStrike)
                {
                    // Heals player for 5% of max health
                    player.GetComponent<PlayerManager>().Heal(5);
                    Debug.Log("Healed");
                }

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
