using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BubbleBurn : MonoBehaviour
{
    public float burnDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        burnDamage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().attackDamage / 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator BubbleBurnDamage(Collider enemy)
    {
        float elapsedTime = 0;
        float burnDuration = 3.0f;
        while (elapsedTime < burnDuration)
        {
            elapsedTime += Time.deltaTime;
            enemy.GetComponent<EnemyStats>()._currentHealth - burnDamage;
            yield return null;
        }

}
