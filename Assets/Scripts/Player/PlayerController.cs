using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PSEMO.Camera;
using PSEMO.Core.Management;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour, IStateMachineUser, InputSystem_Actions.IPlayerActions
    {
        public PlayerSO data;
        [SerializeField] private List<Transform> groundChecks;

        private InputSystem_Actions inputActions;

        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public Animator animator;

        private StateMachine stateMachine;

        private Vector3 respawnPos;

        //Inputs
        [HideInInspector] public float moveInput = 0;
        [HideInInspector] public bool upInput = false;
        [HideInInspector] public bool dashInput = false;

        //Move
        [HideInInspector] public Vector3 initialScale;
        [HideInInspector] public int facing = 1;

        //Jump
        [HideInInspector] public bool isGrounded = true;
        [HideInInspector] public float coyoteTimeCounter = 0;
        [HideInInspector] public float jumpBufferCounter = 0;
        [HideInInspector] public int jumpsLeft = 0;
        [HideInInspector] public bool hasJumped = false;

        //Dash
        [HideInInspector] public bool canDash = true;
    
        void Awake()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Player.AddCallbacks(this);

            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

            InitializeStateMachine();
        }

        void Start()
        {
            CameraManager.Instance.AddTarget(transform, data.camDivisor);

            respawnPos = transform.position;
            jumpsLeft = data.jumpCount;
            initialScale = transform.localScale;
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
            UpdateTimers();

            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private void UpdateTimers()
        {
            if (isGrounded)
            {
                coyoteTimeCounter = data.coyoteTime;
                if (rb.linearVelocity.y <= 0.1f)
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

        public virtual void Run()
        {
            float targetSpeed = moveInput * data.speed;
            rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);

            if (moveInput != 0)
            {
                facing = moveInput >= 0? 1 : -1;

                transform.localScale = new Vector3(
                    initialScale.x * facing,
                    initialScale.y,
                    initialScale.z);
            }
        }

        private void Die()
        {
            transform.position = respawnPos;
            rb.linearVelocity = Vector2.zero;

            moveInput = 0;
            dashInput = false;
            upInput = false;
        
            stateMachine.SetState(new IdleState(this, animator)); 
        }

        private void SetRespawnPos(Vector3 pos)
        {
            respawnPos = pos;
        }

        //===== STATE MACHINE =====
        void InitializeStateMachine()
        {
            stateMachine = new StateMachine();

            var idleState = new IdleState(this, animator);
            var runState = new RunState(this, animator);
            var jumpState = new JumpState(this, animator);
            var fallState = new FallState(this, animator);
            var dashState = new DashState(this, animator);

            void At(IState from, IState to, Func<bool> condition) =>
                stateMachine.AddTransition(from, to, new FuncPredicate(condition));

            void Any(IState to, Func<bool> condition) =>
                stateMachine.AddAnyTransition(to, new FuncPredicate(condition));

            At(idleState, runState, () => Mathf.Abs(moveInput) >= 0.1f);

            At(runState, idleState, () => Mathf.Abs(moveInput) < 0.1f);

            At(fallState, idleState, () => isGrounded && Mathf.Abs(moveInput) < 0.1f);
            At(fallState, runState, () => isGrounded && Mathf.Abs(moveInput) >= 0.1f);
        
            At(dashState, idleState, () => !dashState.IsDashing() && isGrounded && Mathf.Abs(moveInput) < 0.1f);
            At(dashState, runState, () => !dashState.IsDashing() && isGrounded && Mathf.Abs(moveInput) >= 0.1f);
            At(dashState, fallState, () => !dashState.IsDashing() && !isGrounded);

            Any(jumpState, () => jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || jumpsLeft > 0));

            Any(dashState, () => dashInput && canDash);

            Any(fallState, () => rb.linearVelocityY < -0.1f);

            stateMachine.SetState(idleState);
        }
        //=========================

        //======== INPUTS =========
        public void OnMove(InputAction.CallbackContext context)
        {
            if (data.ableToRun)
            {
                moveInput = context.ReadValue<float>();
            }
        }

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed && data.ableToJump)
            {
                upInput = true;
                jumpBufferCounter = data.jumpBufferTime;
            }
            else if (context.canceled)
            {
                upInput = false;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && data.ableToDash)
            {
                dashInput = true;
            }
            else if (context.canceled)
            {
                dashInput = false;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && data.ableToInteract)
            {
                Collider2D hit = Physics2D.OverlapCircle(transform.position, data.interactionRadius, data.interactionLayer);

                if (hit != null && hit.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteracted();
                }
            }
        }
        //=========================
    }
}