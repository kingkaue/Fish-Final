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
    public float fireRate = 5f;
    Animator animator;
    public GameObject RightFistHitbox;
    public GameObject LeftFistHitbox;
    private bool canSwing = true;
    private void Start()
    {

        // Setting basic stats
        className = "Fighter";

        // Health and damage managed inside PlayerManager script so sets variables there
        classBaseMaxHealth = 250;
        GetComponent<PlayerManager>().InitializeHealth(classBaseMaxHealth);

        classBaseAttackDamage = 8;
        GetComponent<PlayerManager>().baseAttackDamage = classBaseAttackDamage; // Sets class base damage as starting damage
        GetComponent<PlayerManager>().SetAttackDamage(1, classBaseAttackDamage);

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
        if (basicAttack.triggered && canSwing)
        {
            Attack();
        }

        //Debug.Log(canSwing);
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f); // Adjust the delay to match your animation
        animator.SetBool("IsPunching", false);
    }

    private void Attack()
    {
        StartCoroutine(ActivateHitBoxes());
        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ActivateHitBoxes()
    { 
        // start attack       
        animator.SetBool("IsPunching", true);
        nextFireTime = Time.time + fireRate;
        canSwing = false;
        //Debug.Log("start attack");

        // activate attack hitboxes at top of swing
        yield return new WaitForSeconds(.45f);
        RightFistHitbox.SetActive(true);
        LeftFistHitbox.SetActive(true);
        //Debug.Log("hitbox active");
        
        // Deactivate hitboxes at bottom of swing
        yield return new WaitForSeconds(.55f); // Duration of hitbox activation
        RightFistHitbox.SetActive(false);
        LeftFistHitbox.SetActive(false);
        //Debug.Log("disable hitbox");

        // allow player to swing again once firerate cooldown is over
        yield return new WaitForSeconds(fireRate - 1f);
        canSwing = true;
        //Debug.Log("end attack");
        
    }
}