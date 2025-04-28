using UnityEngine;

public class Dagger : MonoBehaviour
{
    public float daggerDamage;
    private int maxPierceCount = 3;
    private int currentPierceCount = 0;

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

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

            if (GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterClass_Rogue>().canPierce)
            {
                currentPierceCount++;

                if (currentPierceCount >= maxPierceCount)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
