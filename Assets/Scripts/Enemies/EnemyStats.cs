using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Identification")]
    public string EnemyType = "BasicEnemy"; // Set this in inspector for each prefab

    [Header("Health Settings")]
    public float MaxHealth = 10f;
    [SerializeField] public float _currentHealth;
    public HealthBarScript HealthBar;
    public GameObject CollectableDrop;

    [Header("Movement Settings")]
    [SerializeField] private float stoppingDistance = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    // Properties for safe access
    public float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (HealthBar != null) HealthBar.UpdateHealthBar(MaxHealth, _currentHealth);
        }
    }

    // Components
    private Animator _animator;
    private NavMeshAgent _agent;
    private bool _isDying = false;

    void Start()
    {
        InitializeComponents();
        CurrentHealth = MaxHealth; // Uses the property setter
    }

    private void InitializeComponents()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        // Configure NavMeshAgent
        if (_agent != null)
        {
            _agent.stoppingDistance = stoppingDistance;
            _agent.updateUpAxis = false;
            _agent.baseOffset = 0.1f;
            _agent.acceleration = 8f;
        }
    }

    void Update()
    {
        if (CurrentHealth <= 0 && !_isDying)
        {
            _isDying = true;
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"TakeDamage called on {gameObject.name} with damage: {damage}");

        if (_isDying) return;

        CurrentHealth -= damage;

        if (hitSound != null && audioSource != null)
        {
            Debug.Log($"Playing hit sound: {hitSound.name}, Volume: {audioSource.volume}");

            audioSource.PlayOneShot(hitSound);
        }

        // Trigger hit animation if needed
        if (_animator != null)
        {
            _animator.SetTrigger("Hit");
        }


    }

    private IEnumerator Die()
    {
        // Drop collectable if specified
        if (CollectableDrop != null)
        {
            Debug.Log($"Spawning collectable at position: {transform.position}");
            Instantiate(CollectableDrop, transform.position, Quaternion.identity);
        }

        // Play death animation
        if (_animator != null)
        {
            _animator.SetBool("isdead", true);
        }

        // Disable collider and agent
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        if (_agent != null) _agent.enabled = false;

        // Wait for animation (or fixed time if no animation)
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    // For save system integration
    public EnemySaveData GetSaveData()
    {
        return new EnemySaveData
        {
            EnemyType = this.EnemyType,
            Position = transform.position,
            Rotation = transform.rotation,
            CurrentHealth = this.CurrentHealth
        };
    }

    public void ApplyBubbleBurn(float damage, float duration)
    {
        Debug.Log("Applying burn");
        StartCoroutine(HandleBurn(damage, duration));
    }

    private IEnumerator HandleBurn(float damage, float duration)
    {
        float tickInterval = 0.5f;
        int ticks = Mathf.FloorToInt(duration / tickInterval);

        for (int i = 0; i < ticks; i++)
        {
            if (this != null)
            {
                _currentHealth -= damage;
                HealthBar.UpdateHealthBar(MaxHealth, _currentHealth);
                yield return new WaitForSeconds(tickInterval);
            }
        }
    }
}


// For save system (could be in a separate file)
[System.Serializable]
public struct EnemySaveData
{
    public string EnemyType;
    public Vector3 Position;
    public Quaternion Rotation;
    public float CurrentHealth;
}