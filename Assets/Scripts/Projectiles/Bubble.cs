using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float coreDamage = 3f;
    public float splashDamageRadius = 4f;
    public float splashDamage = 1f;
    void SplashDamage(Collider hitEnemy)
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, splashDamageRadius);
        foreach(Collider c in enemyColliders)
        {
            if (c.tag == "Enemy" && c != hitEnemy)
            {
                c.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(c.GetComponent<EnemyStats>()._currentHealth - splashDamage, 0);
                c.GetComponent<EnemyStats>()._healthbar.UpdateHealthBar(c.GetComponent<EnemyStats>()._maxHealth, c.GetComponent<EnemyStats>()._currentHealth);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(other.GetComponent<EnemyStats>()._currentHealth - coreDamage, 0);
            other.GetComponent<EnemyStats>()._healthbar.UpdateHealthBar(other.GetComponent<EnemyStats>()._maxHealth, other.GetComponent<EnemyStats>()._currentHealth);
            SplashDamage(other);
            Destroy(this.gameObject);
        }
    }
}
