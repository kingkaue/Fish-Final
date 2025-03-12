using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
public class EnemyStats : MonoBehaviour
{
    public float _maxHealth = 10;
    public float _currentHealth;
    public HealthBarScript _healthbar;
    public GameObject collectable;

    void Start()
    {
        _currentHealth = _maxHealth;
        _healthbar.UpdateHealthBar(_maxHealth, _currentHealth);
    }

    private void LateUpdate()
    {
        if(_currentHealth == 0)
        {
            GameObject pickup = Instantiate(collectable, transform.position, Quaternion.identity);
            Vector3 collectablePosition = transform.position;
            collectablePosition.x = transform.position.x;
            collectablePosition.z = transform.position.z;
            pickup.transform.position = collectablePosition;
            Destroy(gameObject);
        }
    }
}