using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    Animator animator;
    public bool isPC;
    private Rigidbody rb;


    [Header("Movement")]
    public float speed;
    private Vector2 move;

    // Only used for rogue movement
    private bool isDashing;
    private bool canDash = true;

    [Header("Aiming")]
    private Vector2 mouseLook;
    private Vector2 joystickLook;
    private Vector3 rotationTarget;
    public Vector3 aimDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mouseLook = context.ReadValue<Vector2>();
    }

    public void OnJoystickLook(InputAction.CallbackContext context)
    {
        joystickLook = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // Checks first to see if player is playing rogue
        if (GetComponent<CharacterClass_Rogue>() != null)
        {
            if (canDash)
            {
                StartCoroutine(Dash());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Makes sure player can't do any inputs while dashing
        if (isDashing)
        {
            return;
        }

        // If using mouse and keyboard, sets rotation target to mouse
        if (isPC)
        {
            RaycastHit hit;

            // Creates ray pointing to mouse locatiin
            Ray ray = Camera.main.ScreenPointToRay(mouseLook);

            // Rotates towards mouse pointer
            if (Physics.Raycast(ray, out hit))
            {
                rotationTarget = hit.point;
            }
        }
        // If using controller
        else
        {
            // If no input on right joystick, rotate with left stick
            if (joystickLook.x == 0 && joystickLook.y == 0)
            {
                MovePlayer();
            }
            else
            {
                MovePlayerWithAim();
            }
        }

        if (move == Vector2.zero)
        {
            animator.SetBool("IsRunning", false);
        }
    }

    void FixedUpdate()
    {
        // Makes sure player can't do any inputs while dashing
        if (isDashing)
        {
            return;
        }

        if (isPC)
        {
            MovePlayerWithAim();
        }
        else
        {
            // If no input on right joystick, rotate with left stick
            if (joystickLook.x == 0 && joystickLook.y == 0)
            {
                MovePlayer();
            }
            else
            {
                MovePlayerWithAim();
            }
        }
    }

    // Movement and rotation handled by left joystick
    public void MovePlayer()
    {
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y).normalized;

        if (moveDirection != Vector3.zero)
        {
            rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(moveDirection), 0.15f);
            animator.SetBool("IsRunning", true);
        }

        rb.linearVelocity = moveDirection * speed;
    }

    // Movement and rotation handled separately
    public void MovePlayerWithAim()
    {
        if (isPC)
        {
            var lookPos = rotationTarget - rb.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                rb.rotation = Quaternion.Slerp(rb.rotation, rotation, 0.15f);
            }
        }
        else
        {
            aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }

        Vector3 moveDirection = new Vector3(move.x, 0f, move.y).normalized;

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("IsRunning", true);  // <-- Ensure animation plays
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        rb.linearVelocity = moveDirection * speed;
    }

    // Only used for rogue class
    private IEnumerator Dash()
    {
        // Gets variables from rogue script
        CharacterClass_Rogue rogueScript = GetComponent<CharacterClass_Rogue>();
        float dashSpeed = rogueScript.dashSpeed;
        float dashDuration = rogueScript.dashDuration;
        float dashCooldown = rogueScript.dashCooldown;

        isDashing = true;
        canDash = false;

        Vector3 dashDirection = new Vector3(move.x, 0f, move.y).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        // Dashes for dashDuration amount of seconds
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        // Manages dash cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


}
