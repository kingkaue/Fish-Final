using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
public class EnemyStats : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3;
    [SerializeField] private float _currentHealth;
    [SerializeField] private HealthBarScript _healthbar;

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            _currentHealth = Mathf.Max(_currentHealth - 1, 0); // Ensure health doesn't go below 0
            _healthbar.UpdateHealthBar(_maxHealth, _currentHealth); // Update health bar
            Debug.Log("Projectile Collision With Enemy");
        }
    }
}