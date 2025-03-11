using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClass_Rogue : CharacterClass
{
    [SerializeField] GameObject daggerPrefab;
    [SerializeField] Transform daggerOrigin;
    public float daggerSpeed = 7f;
    public float fireRate = 0.7f;
    public int numDaggers = 2;

    private float nextFireTime = 0f;
    private PlayerInput playerInput;
    private InputAction basicAttack;
    private Vector3 direction;

    private void Start()
    {
        className = "Rogue";
        maxHealth = 200;
        GetComponent<PlayerManager>().currentHealth = maxHealth;
        moveSpeed = 10;
        attackDamage = 8;
        playerInput = GetComponent<PlayerInput>();

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
            if (basicAttack.ReadValue<float>() != 0 && Time.time >= nextFireTime)
            {
                ShootDaggers();
                nextFireTime = Time.time + fireRate;
                Debug.Log("Shooting bubble towards " + aim.x + " " + aim.z);
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

        for (int i = 0; i < numDaggers; i++)
        {
            GameObject dagger = Instantiate(daggerPrefab, daggerOrigin.position, Quaternion.identity);
            Rigidbody daggerRb = dagger.GetComponent<Rigidbody>();
           daggerRb.linearVelocity = GetDaggerDirection(numDaggers) * daggerSpeed;
        }
    }

    Vector3 GetDaggerDirection(int numOfDaggers)
    {
        Vector3 targetDir = direction;
        targetDir = new Vector3(targetDir.x + Random.Range(-3f, 3), 0f, targetDir.z + Random.Range(-3f, 3));
        return targetDir;
    }
}
