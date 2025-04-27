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
        if (isPC)
        {
            if (basicAttack.triggered && Time.time >= nextFireTime)
            {

                if (canswing)
                {
                    StartCoroutine(ActivateHitBoxes());
                    StartCoroutine(ResetAttackAnimation());
                }
            }
        }
        else
        {
            if (basicAttack.triggered && Time.time >= nextFireTime)
            {

                if (canswing)
                {
                    StartCoroutine(ActivateHitBoxes());
                    StartCoroutine(ResetAttackAnimation());
                }
            }
        }
    }

    private IEnumerator ActivateHitBoxes()
    {
        canswing = false;
        animator.SetBool("IsPunching", true);
        yield return new WaitForSeconds(.45f);
        while (animator.GetBool("IsPunching"))
        {
            RightFistHitbox.SetActive(true);
            LeftFistHitbox.SetActive(true);
        }
        nextFireTime = Time.time + fireRate; // Update the next allowed attack time
        yield return new WaitForSeconds(fireRate); // Duration of hitbox activation
        RightFistHitbox.SetActive(false);
        LeftFistHitbox.SetActive(false);
        canswing = true;
    }
}