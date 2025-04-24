using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
   
    public float _maxHealth = 10;
    public float _currentHealth;
    public HealthBarScript _healthbar;
    public GameObject collectable;
    private Animator animator;
    private bool isDying = false;  // Flag to prevent multiple executions

    [SerializeField] private float stoppingDistance = 1.5f;
    private GameObject destination;
    private NavMeshAgent agent;
    void Start()
    {
        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        // Critical agent settings
        agent.stoppingDistance = stoppingDistance;
        agent.updateUpAxis = false;
        agent.baseOffset = 0.1f;
        agent.acceleration = 8f;
        _currentHealth = _maxHealth;
        _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    private void LateUpdate()
    {
        if (_currentHealth <= 0 && !isDying) // Check if already dying
        {
            isDying = true; // Set the flag. Now it won't execute again. 
            StartCoroutine(dying());
        }
    }

    private IEnumerator dying()
    {
        GameObject pickup = Instantiate(collectable, transform.position, Quaternion.identity);
        animator.SetBool("isdead", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
