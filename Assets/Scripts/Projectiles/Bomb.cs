using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float grenadeDamage;
    [SerializeField] float delay = 3f;
    [SerializeField] float blastRadius = 5f;
    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grenadeDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage * 1.5f;
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Debug.Log("Boom");

        // Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Get nearby enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider c in enemies)
        {
            // Damage
            if (c.tag == "Enemy")
            {
                c.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(c.GetComponent<EnemyStats>()._currentHealth - grenadeDamage, 0);
                c.GetComponent<EnemyStats>().HealthBar.UpdateHealthBar(c.GetComponent<EnemyStats>().MaxHealth, c.GetComponent<EnemyStats>()._currentHealth);
            }
        }

        // Remove grenade
        Destroy(gameObject);
    }
}
