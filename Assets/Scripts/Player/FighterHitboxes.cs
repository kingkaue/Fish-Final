using UnityEngine;

public class FighterHitboxes : MonoBehaviour
{
    public float coreDamage = 2;

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
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(other.GetComponent<EnemyStats>()._currentHealth - coreDamage, 0);
            other.GetComponent<EnemyStats>()._healthbar.UpdateHealthBar(other.GetComponent<EnemyStats>()._maxHealth, other.GetComponent<EnemyStats>()._currentHealth);
        }
    }
}
