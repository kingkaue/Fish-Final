using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class CharacterClass_FIghter : CharacterClass
{
    [Header("Basic Stats")]
    private PlayerInput playerInput;
    private InputAction basicAttack;
    private float nextFireTime = 0f;
    public float fireRate = 0.7f;
    Animator animator;

    private void Start()
    {
        // Setting basic stats
        className = "Fighter";
        maxHealth = 250;
        GetComponent<PlayerManager>().currentHealth = maxHealth;
        moveSpeed = 10;
        attackDamage = 8;
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        // Changes basic attack input control
        if (isPC)
        {
            basicAttack = playerInput.actions["BasicAttack_mouse"];
        }
        else
        {
            basicAttack = playerInput.actions["BasicAttack_joystick"];
        }
    }

    void Update()
    {
        Attack();
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Adjust the delay to match your animation
        animator.SetBool("IsPunching", false);
    }

    private void Attack()
    {
        if (isPC)
        {
            // Shoots if holding down LMB
            if (basicAttack.ReadValue<float>() != 0 && Time.time >= nextFireTime)
            {
                animator.SetBool("IsPunching", true);
                nextFireTime = Time.time + fireRate;
                StartCoroutine(ResetAttackAnimation());
            }
        }
        else
        {
            // Shoots when aiming with right joystick
            if (basicAttack.ReadValue<Vector2>() != Vector2.zero && Time.time >= nextFireTime)
            {
                animator.SetBool("IsPunching", true);
                nextFireTime = Time.time + fireRate;
                StartCoroutine(ResetAttackAnimation());
            }
        }

    }



}
