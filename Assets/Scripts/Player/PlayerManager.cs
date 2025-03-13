using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
    [Header ("Health Management")]
    [SerializeField] float baseMaxHealth;
    [SerializeField] float currentMaxHealth;
    [SerializeField] float currentHealth;
    public float healthMultiplier;

    [Header ("Damage Management")]
    public float baseAttackDamage;
    public float attackDamage;
    public float damageMultiplier;
    public bool isInvincible;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Player Health is " + currentHealth);
        damageMultiplier = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Makes sure current health isn't over the max health
        if (currentMaxHealth < currentHealth)
        {
            currentHealth = currentMaxHealth;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player Health is dead");
            Destroy(this.gameObject);
        }
    }

    ///////////////////////////////////////////
    //----- Damage Management Functions -----//
    ///////////////////////////////////////////

    // Called in other scripts to change damage multiplier
    public void SetDamageMultiplier(float damageMultiplierAdd)
    {
        if (damageMultiplier == 0)
        {
            damageMultiplier = 1f;
        }
        else
        {
            damageMultiplier = damageMultiplier * damageMultiplierAdd;
        }
    }

    // Called in other scripts to change base damage
    public void SetBaseDamage(float baseAdd)
    {
        baseAttackDamage = baseAttackDamage + baseAdd;
    }

    // Sets attack damage based on base damage value and multiplier
    public void SetAttackDamage(float multiplier, float baseDamage)
    {
        attackDamage = baseDamage * multiplier;
    }

    ///////////////////////////////////////////
    //----- Health Management Functions -----//
    ///////////////////////////////////////////

    public void InitializeHealth(float classHealth)
    {
        baseMaxHealth = classHealth;
        currentMaxHealth = classHealth;
        currentHealth = classHealth;
    }
    // Called in other scripts to change health multiplier
    public void SetHealthMultiplier(float healthMultiplierAdd)
    {
        healthMultiplier = healthMultiplier * healthMultiplierAdd;
    }

    // Called in other scripts to change max health
    public void MultiplyMaxHealth(float healthmult)
    {
        float oldCurrentHealth = currentHealth;
        currentMaxHealth = currentMaxHealth * healthmult;

        // In cases like glass cannon where players take a health down multiplier,
        // this prevents reduction of current health
        if ((currentHealth * healthmult) > oldCurrentHealth)
        {
            currentHealth = currentHealth * healthmult;
        }

        // Prevents player from ahving more health than max
        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }

    }

    // Called in other scripts to heal player
    public void Heal(float percentageOfMaxHealth)
    {
        currentHealth = currentHealth + (currentMaxHealth * percentageOfMaxHealth);
    }

    // Called in other scripts to deal damage to player
    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;
        StartCoroutine(InvincibilityFrames(0.25f));
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
}
