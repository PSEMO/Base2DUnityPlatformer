using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] PlayerSO data;
    [SerializeField] List<Transform> groundChecks;

    private Vector3 respawnPos;

    InputSystem_Actions inputActions;

    float moveInput = 0;
    int facingDirection = 1;
    bool sprintInput = false;
    bool downInput = false; //unused rn
    bool interactInput = false; //unused rn

    bool isDashing = false;
    bool canDash = true;

    Rigidbody2D rb;

    bool isGrounded = true;
    float coyoteTimeCounter = 0;
    float jumpBufferCounter = 0;
    bool hasJumped = false;
    int jumpsLeft;

    public PlayerState CurrentState { get; private set; }

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.AddCallbacks(this);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CameraManager.Instance.AddTarget(transform, data.camDivisor);

        respawnPos = transform.position;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        Events.OnPlayerDeath += Die;
        Events.OnCheckPointReached += SetRespawnPos;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
        Events.OnPlayerDeath -= Die;
        Events.OnCheckPointReached -= SetRespawnPos;
    }

    void OnDestroy()
    {
        inputActions.Player.RemoveCallbacks(this);
        inputActions.Dispose();
        CameraManager.Instance.RemoveTarget(transform);
    }

    void Update()
    {
        isGrounded = IsOnGround();
        UpdateState();
    }

    private void UpdateState()
    {
        if (isDashing)
        {
            CurrentState = PlayerState.Dashing;
        }
        else if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                CurrentState = PlayerState.Jumping;
            }
            else
            {
                CurrentState = PlayerState.Falling;
            }
        }
        else if (moveInput != 0)
        {
            CurrentState = sprintInput ? PlayerState.Running : PlayerState.Walking;
        }
        else
        {
            CurrentState = PlayerState.Idle;
        }
    }

    void FixedUpdate()
    {
        JumpCheckers();
        
        if (isDashing) return;

        if (jumpBufferCounter > 0f && jumpsLeft > 0)
        {
            ExecuteJump();
        }

        float velocityX = moveInput * data.speed * (sprintInput? data.sprintMultiplier : 1.0f);
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
        if (moveInput != 0)
        {
            int newFacingDirection = moveInput >= 0 ? 1 : -1;
            if (newFacingDirection != facingDirection)
            {
                facingDirection = newFacingDirection;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferCounter = data.jumpBufferTime;

            if (jumpsLeft > 0)
            {
                ExecuteJump();
            }
        }

        if (hasJumped && context.canceled && data.variableJump && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * data.variableJumpMult);
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        
        rb.linearVelocity = new Vector2(facingDirection * data.dashForce, 0f);
        
        yield return new WaitForSeconds(data.dashDuration);
        
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    private void JumpCheckers()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = data.coyoteTime;
            if (rb.linearVelocity.y <= 0f)
            {
                hasJumped = false;
                jumpsLeft = data.jumpCount;
                canDash = true;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;

            if (coyoteTimeCounter <= 0f && !hasJumped && jumpsLeft == data.jumpCount)
            {
                jumpsLeft--;
            }
        }

        if (jumpBufferCounter > 0f)
        {
            jumpBufferCounter -= Time.deltaTime;
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
        jumpsLeft--;
    }

    private void Die()
    {
        transform.position = respawnPos;
        rb.linearVelocity = Vector2.zero;
    }

    private void SetRespawnPos(Vector3 pos)
    {
        respawnPos = pos;
    }
}