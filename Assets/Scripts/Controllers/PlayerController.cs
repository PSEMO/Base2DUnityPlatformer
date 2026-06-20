using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] PlayerSO data;
    [SerializeField] List<Transform> groundChecks;

    InputSystem_Actions inputActions;

    float moveInput = 0;
    bool upInput = false;
    bool downInput = false;
    bool sprintInput = false;
    bool interactInput = false;

    Rigidbody2D rb;

    bool isGrounded = true;
    float coyoteTimeCounter = 0;
    float jumpBufferCounter = 0;
    bool hasJumped = false;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.AddCallbacks(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CameraManager.instance.AddTarget(transform, data.camDivisor);
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void OnDestroy()
    {
        inputActions.Player.RemoveCallbacks(this);
        inputActions.Dispose();
        CameraManager.instance.RemoveTarget(transform);
    }

    void Update()
    {
        isGrounded = IsOnGround();
        JumpCheckers();
    }

    void FixedUpdate()
    {
        float velocityX = moveInput * data.speed * (sprintInput? data.sprintMultiplier : 1.0f);
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        upInput = context.performed;

        if (context.performed)
        {
            jumpBufferCounter = data.jumpBufferTime;

            if (coyoteTimeCounter > 0f && !hasJumped)
            {
                ExecuteJump();
            }
        }

        if (context.canceled && data.variableJump && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintInput = true;
        }
        else if (context.canceled)
        {
            sprintInput = false;
        }
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        downInput = context.performed;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        interactInput = context.performed;
    }

    private void JumpCheckers()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = data.coyoteTime;
            hasJumped = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !hasJumped)
        {
            ExecuteJump();
        }
    }

    private bool IsOnGround()
    {
        foreach (Transform check in groundChecks)
        {
            if (check == null) continue;

            RaycastHit2D hit = Physics2D.Raycast(check.position, Vector2.down, data.groundCheckDistance, data.groundLayer);

            if (hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }

    void ExecuteJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.jumpForce);
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
        hasJumped = true;
    }
}