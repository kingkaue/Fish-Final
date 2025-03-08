using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    public float speed;
    private Vector2 move;
    private Vector2 mouseLook;
    private Vector2 joystickLook;
    private Vector3 rotationTarget;
    public bool isPC;

    private void Start()
    {
        animator = GetComponent<Animator>();
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

    // Update is called once per frame
    void Update()
    {
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

            MovePlayerWithAim();
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

        if(move == Vector2.zero)
        {
            animator.SetBool("IsRunning", false);
        }
    }

    // Movement and rotation handled by left joystick
    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            animator.SetBool("IsRunning", true);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Movement and rotation handled separately
    public void MovePlayerWithAim()
    {
        if (isPC)
        {
            var lookPos = rotationTarget - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);

            Vector3 aimDirection = new Vector3(rotationTarget.x, 0f, rotationTarget.z);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
            }
        }
        else
        {
            Vector3 aimDirection = new Vector3(joystickLook.x, 0f, joystickLook.y);

            if (aimDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(aimDirection), 0.15f);
            }
        }

        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
