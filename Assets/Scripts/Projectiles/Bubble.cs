using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class Bubble : MonoBehaviour
{
    public float coreDamage;
    public float splashDamageRadius = 4f;
    public float splashDamage;

    [Header("Split Shot")]
    [SerializeField] Transform splitOrigin;
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] float spreadAngle;
    [SerializeField] int numBubbles;
    [SerializeField] bool isSplit = false;
    [SerializeField] bool isBubbleBurn = false;

    void Update()
    {
        coreDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage;
        splashDamage = coreDamage * 0.5f;
    }

    void SplashDamage(Collider hitEnemy)
    {
        // Checks to see if there are other enemies in splash damage range
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, splashDamageRadius);
        foreach (Collider c in enemyColliders)
        {
            // Deals damage to enemies in range
            if (c.tag == "Enemy" && c != hitEnemy)
            {
                c.GetComponent<EnemyStats>()._currentHealth = Mathf.Max(c.GetComponent<EnemyStats>()._currentHealth - splashDamage, 0);
                c.GetComponent<EnemyStats>().HealthBar.UpdateHealthBar(c.GetComponent<EnemyStats>().MaxHealth, c.GetComponent<EnemyStats>()._currentHealth);
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
            other.GetComponent<EnemyStats>().HealthBar.UpdateHealthBar(other.GetComponent<EnemyStats>().MaxHealth, other.GetComponent<EnemyStats>()._currentHealth);

            // Deals splash damage
            SplashDamage(other);
            Debug.Log("Dealt " + coreDamage + " core damage and " + splashDamage + " splash damage");
            if (isSplit == true)
            {
                SplitShot(other);
            }
            if (isBubbleBurn == true)
            {
                if (other.GetComponent<EnemyStats>() != null)
                {
                    other.GetComponent<EnemyStats>().ApplyBubbleBurn(coreDamage / 10, 3f);
                }
            }
            Destroy(this.gameObject);
        }
    }

    public void ActivateSplit()
    {
        isSplit = true;
    }

    public void ActivateBurn()
    {
        isBubbleBurn = true;
    }

    void SplitShot(Collider hitEnemy)
    {
        // Equally spaces distance between daggers depending on number of daggers
        float angleBetweenBubbles = spreadAngle / (numBubbles - 1);
        float startAngle = -spreadAngle / 2;
        Quaternion originalRotation = splitOrigin.rotation;

        // Spawns each dagger
        for (int i = 0; i < numBubbles; i++)
        {
            float angle = startAngle + (angleBetweenBubbles * i);

            // Shoots dagger at certain angle
            splitOrigin.transform.Rotate(0, angle, 0);
            GameObject splitBubble = Instantiate(bubblePrefab, splitOrigin.position, Quaternion.identity);
            splitBubble.GetComponent<Bubble>().isSplit = false;
            Physics.IgnoreCollision(splitBubble.GetComponent<SphereCollider>(), hitEnemy);
            Rigidbody splitBubbleRb = splitBubble.GetComponent<Rigidbody>();
            splitBubbleRb.AddForce(splitOrigin.transform.forward * 500);

            // Resets rotation of dagggerOrigin to work properly
            splitOrigin.rotation = originalRotation;
        }
    }
}
