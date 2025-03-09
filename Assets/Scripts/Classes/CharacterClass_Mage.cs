using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClass_Mage : CharacterClass
{
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform bubbleOrigin;
    [SerializeField] float bubbleSpeed = 5f;
    [SerializeField] float fireRate = 0.5f;

    private float nextFireTime = 0f;
    private PlayerInput playerInput;
    private InputAction basicAttack;

    void Start()
    {
        // Initializing class stats
        className = "Mage";
        maxHealth = 150;
        GetComponent<PlayerManager>().currentHealth = maxHealth;
        moveSpeed = 8;
        attackDamage = 10;
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
        Rigidbody bubbleRb = bubble.GetComponent<Rigidbody>();
        bubbleRb.linearVelocity = direction * bubbleSpeed;
        Debug.Log("Bubble Speed is " + bubbleRb.linearVelocity);
    }
}
