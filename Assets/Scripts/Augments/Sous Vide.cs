using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SousVide : MonoBehaviour
{
    public float damagePerSecond;
    private float auraRadius = 7f;
    private float auraDamage;

    private SphereCollider auraCollider;

    private void OnEnable()
    {
        // Initialized collider stats
        auraCollider = gameObject.AddComponent<SphereCollider>();
        auraCollider.radius = auraRadius;
        auraCollider.isTrigger = true;
        auraDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage / 10;
    }

    private void Update()
    {
        // Keeps aura on the player
        this.gameObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Starts coroutine for each enemy when they enter the aura
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            AuraDamage(other);
        }
    }

    // Stops coroutine for enemies that leave the aura
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            StopCoroutine(DamageOverTime(other.gameObject));
        }
    }

    // Coroutine that manages damage ticks
    private IEnumerator DamageOverTime(GameObject enemy)
    {
        // While the enemy is over 0 HP, continuously deals damage every 1.5 seconds
        while (enemy.GetComponent<EnemyStats>()._currentHealth > 0)
        {
            enemy.GetComponent<EnemyStats>()._currentHealth -= auraDamage;
            enemy.GetComponent<EnemyStats>().HealthBar.UpdateHealthBar(enemy.GetComponent<EnemyStats>().MaxHealth, enemy.GetComponent<EnemyStats>()._currentHealth);
            yield return new WaitForSeconds(1.5f);

            // When enemy is killed, stop coroutine
            if (enemy == null)
            {
                yield break;
            }
        }
    }

    // Applies damage coroutine to every enemy inside aura radius
    void AuraDamage(Collider hitEnemy)
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, auraRadius);
        foreach (Collider c in enemyColliders)
        {
            if (c.tag == "Enemy")
            {
                StartCoroutine(DamageOverTime(c.gameObject));
            }
        }
    }
}
