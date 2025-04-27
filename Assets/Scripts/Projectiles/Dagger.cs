using UnityEngine;

public class Dagger : MonoBehaviour
{
    public float daggerDamage;

    void Update()
    {
        daggerDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Deals damage to enemy
        if (other.tag == "Enemy")
        {
            // Deals core damage to enemy hit with projectile
            other.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(other.GetComponent<EnemyStats>()._currentHealth - daggerDamage, 0);
            other.GetComponent<EnemyStats>().HealthBar.UpdateHealthBar(other.GetComponent<EnemyStats>().MaxHealth, other.GetComponent<EnemyStats>()._currentHealth);
            Destroy(this.gameObject);
        }
    }

}
