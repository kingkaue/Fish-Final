using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string className;

    // Delegate for damage events
    public delegate void PlayerDamagedEventHandler(float damageTaken, float currentHealth, float maxHealth);
    public event PlayerDamagedEventHandler OnPlayerDamaged;

    public delegate void PlayerDeathEventHandler();
    public event PlayerDeathEventHandler OnPlayerDeath;

    [Header("Health Management")]
    [SerializeField] float baseMaxHealth;
    [SerializeField] public float currentMaxHealth;
    [SerializeField] public float currentHealth;
    public float healthMultiplier;

    [Header("Damage Management")]
    public float baseAttackDamage;
    public float attackDamage;
    public float damageMultiplier;
    public bool isInvincible;

    void Start()
    {
        Debug.Log("Player Health is " + currentHealth);
        damageMultiplier = 1.0f;
    }

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
            OnPlayerDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }

    ///////////////////////////////////////////
    //----- Damage Management Functions -----//
    ///////////////////////////////////////////

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

    public void SetBaseDamage(float baseAdd)
    {
        baseAttackDamage = baseAttackDamage + baseAdd;
    }

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

    public void SetHealthMultiplier(float healthMultiplierAdd)
    {
        healthMultiplier = healthMultiplier * healthMultiplierAdd;
    }

    public void MultiplyMaxHealth(float healthmult)
    {
        float oldCurrentHealth = currentHealth;
        currentMaxHealth = currentMaxHealth * healthmult;

        if ((currentHealth * healthmult) > oldCurrentHealth)
        {
            currentHealth = currentHealth * healthmult;
        }

        if (currentHealth > currentMaxHealth)
        {
            currentHealth = currentMaxHealth;
        }
    }

    public void Heal(float percentageOfMaxHealth)
    {
        try
        {
            if (percentageOfMaxHealth < 0)
            {
                throw new System.ArgumentException("Heal percentage cannot be negative", nameof(percentageOfMaxHealth));
            }

            currentHealth = currentHealth + (currentMaxHealth * percentageOfMaxHealth);
            Debug.Log($"Player healed by {percentageOfMaxHealth * 100}%");
        }
        catch (System.ArgumentException ex)
        {
            Debug.LogError($"Invalid heal attempt: {ex.Message}");
            // Recover by not healing (or could set to minimum heal)
            currentHealth = currentHealth + 0;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible) return;

        try
        {
            if (damage < 0)
            {
                throw new System.ArgumentException("Damage value cannot be negative", nameof(damage));
            }

            currentHealth = currentHealth - damage;
            OnPlayerDamaged?.Invoke(damage, currentHealth, currentMaxHealth);
            StartCoroutine(InvincibilityFrames(0.25f));
        }
        catch (System.ArgumentException ex)
        {
            Debug.LogError($"Invalid damage attempt: {ex.Message}");
            // Recover by not taking damage
            currentHealth = currentHealth - 0;
        }
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
}