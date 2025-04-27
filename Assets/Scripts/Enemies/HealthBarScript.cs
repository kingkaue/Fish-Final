using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Image _healthbarimage;
    [SerializeField] private float _reduceSpeed = 5.0f; // Increased reduce speed for visible update
    private Camera _cam;
    private float _target = 1;

    private void Start()
    {
        _cam = Camera.main;
        _healthbarimage.fillAmount = 1; // Start with a full health bar
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
        Debug.Log("Health at " + currentHealth); // Log the target value
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        _healthbarimage.fillAmount = Mathf.MoveTowards(_healthbarimage.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    }
}
