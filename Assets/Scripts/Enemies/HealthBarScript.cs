using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image _healthbarimage;
    [SerializeField] private float _reduceSpeed = 2.0f;
    private Camera _cam;
    private float _target = 1; 

    private void Start()
    {
        _cam = Camera.main;
    }
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        _healthbarimage.fillAmount = Mathf.MoveTowards(_healthbarimage.fillAmount, _target, _reduceSpeed * Time.deltaTime);
    }
}
