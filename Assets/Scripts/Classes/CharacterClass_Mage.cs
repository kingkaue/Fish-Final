using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClass_Mage : CharacterClass
{
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform bubbleOrigin;
    public float bubbleSpeed = 5f;
    public float fireRate = 0.5f;
    public bool splitShotActivated = false;

    private float nextFireTime = 0f;
    private PlayerInput playerInput;
    private InputAction basicAttack;

    void Start()
    {
        // Initializing class stats
        className = "Mage";
        GetComponent<PlayerManager>().className = className;

        // Health and damage managed inside PlayerManager script so sets variables there
        classBaseMaxHealth = 150;
        GetComponent<PlayerManager>().InitializeHealth(classBaseMaxHealth);

        classBaseAttackDamage = 10;
        GetComponent<PlayerManager>().baseAttackDamage = classBaseAttackDamage; // Sets class base damage as starting damage
        GetComponent<PlayerManager>().SetAttackDamage(1, classBaseAttackDamage);

        // Speed managed in PlayerMovement script so sets variables there
        moveSpeed = 12;
        GetComponent<PlayerMovement>().speed = moveSpeed;

        playerInput = GetComponent<PlayerInput>();

        // Gets basic attack inputs from input system
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

    void Attack()
    {
        if (isPC)
        {
            // Shoots when holding down LMB
            if (basicAttack.ReadValue<float>() != 0 && Time.time >= nextFireTime)
            {
                ShootBubble();
                nextFireTime = Time.time + fireRate;
                Debug.Log("Shooting bubble towards " + aim.x + " " + aim.z);
            }
        }
        else
        {
            // Shoots when aiming with right joystick
            if (basicAttack.ReadValue<Vector2>() != Vector2.zero && Time.time >= nextFireTime)
            {
                ShootBubble();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void ShootBubble()
    {
        Vector3 direction;

        if (isPC)
        {
            // Shoots in direction of mouse
            aim = GetComponent<PlayerMovement>().aimDirection;
            direction = (aim - bubbleOrigin.position).normalized;
        }
        else
        {
            // Shoots in direction of joystick
            Vector2 joystickInput = basicAttack.ReadValue<Vector2>();
            direction = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;
        }

        // Bullet shoots at set direction at speed
        GameObject bubble = Instantiate(bubblePrefab, bubbleOrigin.position, Quaternion.identity);
        if (splitShotActivated == true)
        {
            bubble.GetComponent<Bubble>().ActivateSplit();
            Debug.Log("isSplit = true");
        }
        Rigidbody bubbleRb = bubble.GetComponent<Rigidbody>();
        bubbleRb.linearVelocity = direction * bubbleSpeed;
        Debug.Log("Bubble Speed is " + bubbleRb.linearVelocity);
        Debug.Log("Bubble Direction is " + direction);
    }

}
