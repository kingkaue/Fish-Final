using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private string gameOverSceneName = "Game Over";
    private bool _hasDied = false;

    [System.Serializable]
    public class PlayerState
    {
        public float savedHealth;
        public float savedMaxHealth;
        public string savedClass;
    }

    private void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, currentMaxHealth);
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
            SceneManager.LoadScene(gameOverSceneName);

        }
    }
    public PlayerState GetPlayerState()
    {
        return new PlayerState()
        {
            savedHealth = currentHealth,
            savedMaxHealth = currentMaxHealth,
            savedClass = className
        };
    }

    public void RestorePlayerState(PlayerState state)
    {
        currentHealth = state.savedHealth;
        currentMaxHealth = state.savedMaxHealth;
        className = state.savedClass;
        // You may need to reinitialize components based on class
    }
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

    [Header("Speed Management")]
    public float speedMultiplier;

    [Header("Stats UI")]
    public TMP_Text healthMultUI;
    public TMP_Text damageMultUI;
    public TMP_Text speedMultUI;


    void Start()
    {
        Debug.Log("Player Health is " + currentHealth);

        //set default mult values
        damageMultiplier = 1.0f;
        speedMultiplier = 1f;
        healthMultiplier = 1f;

        //find stats ui in scene
        healthMultUI = GameObject.Find("Health Multiplier UI").GetComponent<TMP_Text>();
        damageMultUI = GameObject.Find("Damage Multiplier UI").GetComponent<TMP_Text>();
        speedMultUI = GameObject.Find("Speed Multiplier UI").GetComponent<TMP_Text>();

        //update stats UI
        UpdateStatsUI();
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
            SceneManager.LoadScene(gameOverSceneName);

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
        UpdateStatsUI();
    }

    public void SetBaseDamage(float baseAdd)
    {
        baseAttackDamage = baseAttackDamage + baseAdd;
        if (attackDamage < baseAttackDamage)
        {
            attackDamage = baseAttackDamage;
        }
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
        MultiplyMaxHealth(healthMultiplier);
        UpdateStatsUI();
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

    public void SetSpeedMultipler(float speedMult)
    {
        speedMultiplier *= speedMult;
        UpdateStatsUI();
    }

    public void Heal(float percentageOfMaxHealth)
    {
        try
        {
            if (percentageOfMaxHealth < 0)
            {
                throw new System.ArgumentException("Heal percentage cannot be negative", nameof(percentageOfMaxHealth));
            }

            OnPlayerDamaged?.Invoke(0f, currentHealth, currentMaxHealth);
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

    public void UpdateStatsUI()
    {
        healthMultUI.text = $"Health Mult: {healthMultiplier}x";
        damageMultUI.text = $"Damage Mult: {damageMultiplier}x";
        speedMultUI.text = $"Speed Mult: {speedMultiplier}x";
        //Debug.Log("Updated stats ui");
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ignored Collision");
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
    }

   
}