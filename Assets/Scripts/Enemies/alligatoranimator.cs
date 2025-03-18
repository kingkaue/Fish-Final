using System.Collections;
using UnityEngine;
public class alligatoranimator : MonoBehaviour
{
    private Animator animator;
    private EnemyStats enemystats;
    private bool isdamagedcheck = false;
    private float previoushealth;
    private GameObject projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        enemystats = GetComponent<EnemyStats>();
        previoushealth = enemystats._currentHealth; // Initialize previous health

    }

    // Update is called once per frame
    void Update()
    {
        if (enemystats._currentHealth < previoushealth && !isdamagedcheck)
        {
            isdamagedcheck = true;
            animator.SetBool("isdamaged", true);
        }
        previoushealth = enemystats._currentHealth; // Update previous health

    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject == projectile)
        {
            Destroy(projectile);
        }
    }
}
