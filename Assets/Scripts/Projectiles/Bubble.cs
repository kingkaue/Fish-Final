using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float coreDamage;
    public float splashDamageRadius = 4f;
    public float splashDamage;

    void Update()
    {
        coreDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage;
        splashDamage = coreDamage * 0.5f;
    }

    void SplashDamage(Collider hitEnemy)
    {
        // Checks to see if there are other enemies in splash damage range
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, splashDamageRadius);
        foreach(Collider c in enemyColliders)
        {
            // Deals damage to enemies in range
            if (c.tag == "Enemy" && c != hitEnemy)
            {
                c.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(c.GetComponent<EnemyStats>()._currentHealth - splashDamage, 0);
                c.GetComponent<EnemyStats>()._healthbar.UpdateHealthBar(c.GetComponent<EnemyStats>()._maxHealth, c.GetComponent<EnemyStats>()._currentHealth);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Deals damage to enemy
        if (other.tag == "Enemy")
        {
            // Deals core damage to enemy hit with projectile
            other.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(other.GetComponent<EnemyStats>()._currentHealth - coreDamage, 0);
            other.GetComponent<EnemyStats>()._healthbar.UpdateHealthBar(other.GetComponent<EnemyStats>()._maxHealth, other.GetComponent<EnemyStats>()._currentHealth);

            // Deals splash damage
            SplashDamage(other);
            Debug.Log("Dealt " + coreDamage + " core damage and " + splashDamage + " splash damage");
            Destroy(this.gameObject);
        }
    }
}
