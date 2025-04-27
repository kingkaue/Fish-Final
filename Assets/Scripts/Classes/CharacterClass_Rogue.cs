using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClass_Rogue : CharacterClass
{
    [Header("Dagger Stats")]
    [SerializeField] GameObject daggerPrefab;
    [SerializeField] Transform daggerOrigin;
    public float daggerSpeed = 7f;
    public float fireRate = 0.7f;
    public int numDaggers = 2;
    [SerializeField] float spreadAngle = 30f;
    
    [Header("Character Stats")]
    private float nextFireTime = 0f;
    private PlayerInput playerInput;
    private InputAction basicAttack;
    private Vector3 direction;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1f;

    [Header ("Augments")]
    public bool bombDrop = false;
    public bool canDropbomb = true;
    [SerializeField] GameObject bomb;

    private void Start()
    {
        // Setting base stats
        className = "Rogue";
        GetComponent<PlayerManager>().className = className;

        // Health and damage managed inside PlayerManager script so sets variables there
        GetComponent<PlayerManager>().InitializeHealth(classBaseMaxHealth);


        GetComponent<PlayerManager>().baseAttackDamage = classBaseAttackDamage; // Sets class base damage as starting damage
        GetComponent<PlayerManager>().SetAttackDamage(1, classBaseAttackDamage);

        // Speed managed in PlayerMovement script so sets variables there
        GetComponent<PlayerMovement>().speed = moveSpeed;
        
        playerInput = GetComponent<PlayerInput>();

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

    private void Update()
    {
        Attack();
    }
    private void Attack()
    {
        if (isPC)
        {
            // Shoots if holding down LMB
            if (basicAttack.ReadValue<float>() != 0 && Time.time >= nextFireTime)
            {
                ShootDaggers();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            // Shoots when aiming with right joystick
            if (basicAttack.ReadValue<Vector2>() != Vector2.zero && Time.time >= nextFireTime)
            {
                ShootDaggers();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void ShootDaggers()
    {
        if (isPC)
        {
            // Shoots in direction of mouse
            aim = GetComponent<PlayerMovement>().aimDirection;
            direction = (aim - daggerOrigin.position).normalized;
        }
        else
        {
            // Shoots in direction of joystick
            Vector2 joystickInput = basicAttack.ReadValue<Vector2>();
            direction = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;
        }

        ShootDaggersInDirection();
    }

    private void ShootDaggersInDirection()
    {
        // Equally spaces distance between daggers depending on number of daggers
        float angleBetweenDaggers = spreadAngle / (numDaggers - 1);
        float startAngle = -spreadAngle / 2;
        Quaternion originalRotation = daggerOrigin.rotation;

        // Spawns each dagger
        for (int i = 0; i < numDaggers; i++)
        {
            float angle = startAngle + (angleBetweenDaggers * i);

            // Shoots dagger at certain angle
            daggerOrigin.transform.Rotate(0, angle, 0);
            GameObject dagger = Instantiate(daggerPrefab, daggerOrigin.position, daggerOrigin.rotation);
            Rigidbody daggerRb = dagger.GetComponent<Rigidbody>();
            daggerRb.AddForce(daggerOrigin.transform.forward * 500);

            // Resets rotation of dagggerOrigin to work properly
            daggerOrigin.rotation = originalRotation;
        }
    }

    public void DropBomb()
    {
        if (bombDrop && canDropbomb)
        {
            Debug.Log("Dropping bomb");
            Instantiate(bomb, transform.position, transform.rotation);
            canDropbomb = false;
        }
    }
}
