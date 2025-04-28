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
    [Header("Animation Settings")]
    public float animationBlendTime = 0.1f; // Smooth transition time
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
        // Force reset animation state
        animator.Play("Attack", 0, 0f);
        animator.SetBool("IsPunching", true);
        yield return new WaitForSeconds(animationBlendTime);

        while (RightFistHitbox.activeSelf || LeftFistHitbox.activeSelf)
        {
            yield return null;
        }
        // Wait for animation to finish
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        animator.SetBool("IsPunching", false);
        yield return new WaitForSeconds(animationBlendTime);

        nextFireTime = Time.time + fireRate;
        isAttacking = false;
        canswing = true;
    }
}