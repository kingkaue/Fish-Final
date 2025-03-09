using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterClass_Mage : CharacterClass
{
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] Transform bubbleOrigin;
    [SerializeField] float bubbleSpeed = 5f;
    private PlayerInput playerInput;
    private InputAction basicAttack;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        basicAttack = playerInput.actions["BasicAttack_mouse"];
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        aim = GetComponent<PlayerMovement>().lookPos;

        if (basicAttack.ReadValue<float>() != 0)
        {
            ShootBubble();

            Debug.Log("Shooting bubble towards " + aim.x + " " + aim.z);
        }

    }

    private void ShootBubble()
    {
        Vector3 direction = (aim - bubbleOrigin.position).normalized;
        GameObject bubble = Instantiate(bubblePrefab, bubbleOrigin.position, Quaternion.identity);

        Rigidbody bubbleRb = bubble.GetComponent<Rigidbody>(); 
        bubbleRb.linearVelocity = direction * bubbleSpeed;
        Debug.Log("Bubble Speed is " + bubbleRb.linearVelocity);
    }
}
