using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Runtime.CompilerServices;

public class CharacterClass_FIghter : CharacterClass
{
    [Header("Basic Stats")]
    private PlayerInput playerInput;
    private InputAction basicAttack;
    private float nextFireTime = 0f;
    public float fireRate = 2f;
    Animator animator;
    public GameObject RightFistHitbox;
    public GameObject LeftFistHitbox;
    private bool canswing = true;
    private bool isAttacking = false;
   
    private void Start()
    {

        // Setting basic stats
        className = "Fighter";
        GetComponent<PlayerManager>().className = className;

        // Health and damage managed inside PlayerManager script so sets variables there
        GetComponent<PlayerManager>().InitializeHealth(classBaseMaxHealth);

        GetComponent<PlayerManager>().baseAttackDamage = classBaseAttackDamage; // Sets class base damage as starting damage
        GetComponent<PlayerManager>().SetAttackDamage(1, classBaseAttackDamage);

        // Speed managed in PlayerMovement script so sets variables there
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
        if (basicAttack.triggered && !isAttacking && Time.time >= nextFireTime)
        {
            StartCoroutine(ActivateHitBoxes());
        }
    }
    // Called via Animation Event at the start of the hit frame
    private void EnableHitboxes()
    {
        RightFistHitbox.SetActive(true);
        LeftFistHitbox.SetActive(true);
    }

    // Called via Animation Event at the end of the hit frame
    private void DisableHitboxes()
    {
        RightFistHitbox.SetActive(false);
        LeftFistHitbox.SetActive(false);
    }
    private IEnumerator ActivateHitBoxes()
    {
        isAttacking = true;
        canswing = false;
        animator.SetBool("IsPunching", true);

        // Wait for animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("IsPunching", false);
        nextFireTime = Time.time + fireRate;
        isAttacking = false;
        canswing = true;
    }
}