using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CharacterClass_FIghter : CharacterClass
{
    [Header("Basic Stats")]
    private PlayerInput playerInput;
    private InputAction basicAttack;
    private float nextFireTime = 0f;
    public float fireRate = 0.7f;
    Animator animator;
    public GameObject RightFistHitbox;
    public GameObject LeftFistHitbox;

    private void Start()
    {
        // Setting basic stats
        className = "Fighter";

        // Health and damage managed inside PlayerManager script so sets variables there
        classBaseMaxHealth = 250;
        GetComponent<PlayerManager>().InitializeHealth(classBaseMaxHealth);
        
        classBaseAttackDamage = 8;
        GetComponent<PlayerManager>().baseAttackDamage = classBaseAttackDamage; // Sets class base damage as starting damage

        // Speed managed in PlayerMovement script so sets variables there
        moveSpeed = 10;
        GetComponent<PlayerMovement>().speed = moveSpeed;

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
            // Check if the mouse button was clicked (not held)
            if (basicAttack.triggered && Time.time >= nextFireTime)
            {
                StartCoroutine(ActivateHitBoxes());
                StartCoroutine(ResetAttackAnimation());
            }
        }
        else
        {
            // Check if the joystick button was pressed (not held)
            if (basicAttack.triggered && Time.time >= nextFireTime)
            {
                StartCoroutine(ActivateHitBoxes());
                StartCoroutine(ResetAttackAnimation());
            }
        }
    }

    private IEnumerator ActivateHitBoxes()
    {

        animator.SetBool("IsPunching", true);
        RightFistHitbox.SetActive(true);
        LeftFistHitbox.SetActive(true);
        nextFireTime = Time.time + fireRate; // Update the next allowed attack time
        yield return new WaitForSeconds(1f); // Duration of hitbox activation
        RightFistHitbox.SetActive(false);
        LeftFistHitbox.SetActive(false);
    }
}