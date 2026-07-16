using UnityEngine;
using PSEMO.Core.StateMachine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IStateMachineUser, IPersistable
    {
        public PlayerSO data;

        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public Collider2D col;
        [HideInInspector] public Animator animator;

        private PlayerInputHandler inputHandler;
        private PlayerSurfaceDetector surfaceDetector;
        private PlayerStateMachineController stateController;

        [HideInInspector] public Vector3 respawnPos;

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

        //Able To
        [HideInInspector] public bool ableToRun;
        [HideInInspector] public bool ableToJump;
        [HideInInspector] public bool ableToDash;
        [HideInInspector] public bool ableToInteract;
        [HideInInspector] public int maxJumpCount;

        //Anim variables
        private static readonly int RunSpeedHash = Animator.StringToHash("RunSpeed");

        void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            surfaceDetector = new PlayerSurfaceDetector(col, data);
            inputHandler = new PlayerInputHandler(this);
            stateController = new PlayerStateMachineController(this, animator);

            ableToRun = data.ableToRun;
            ableToJump = data.ableToJump;
            ableToDash = data.ableToDash;
            ableToInteract = data.ableToInteract;
            maxJumpCount = data.maxJumpCount;
        }

        void Start()
        {
            CameraEvents.InvokeCameraTargetAdded(transform, data.camWeight);

            respawnPos = transform.position;
            jumpsLeft = maxJumpCount;
            initialScale = transform.localScale;
        }

        void OnEnable()
        {
            inputHandler.OnEnable();
            PlayerEvents.OnPlayerDeath += Die;
            PlayerEvents.OnCheckPointReached += SetRespawnPos;
        }

        void OnDisable()
        {
            inputHandler.OnDisable();
            PlayerEvents.OnPlayerDeath -= Die;
            PlayerEvents.OnCheckPointReached -= SetRespawnPos;
        }

        void OnDestroy()
        {
            inputHandler.OnDestroy();
            CameraEvents.InvokeCameraTargetRemoved(transform);
        }

        void Update()
        {
            isGrounded = surfaceDetector.IsOnGround(col.bounds.center);
            UpdateTimers();

            stateController.Update();
        }

        void FixedUpdate()
        {
            stateController.FixedUpdate();
        }

        private void UpdateTimers()
        {
            if (isGrounded)
            {
                coyoteTimeCounter = data.coyoteTime;
                if (rb.linearVelocity.y <= 0.1f)
                {
                    hasJumped = false;
                    jumpsLeft = maxJumpCount;
                    canDash = true;
                }
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;

                if (coyoteTimeCounter <= 0f && !hasJumped && jumpsLeft == maxJumpCount)
                {
                    jumpsLeft--;
                }
            }

            if (jumpBufferCounter > 0f)
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        public bool IsFacingWall() => surfaceDetector.IsFacingWall(col.bounds.center, facing);

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
            
            animator.SetFloat(RunSpeedHash, targetSpeed / data.runAnimBaseSpeed);
        }

        private void Die()
        {
            Respawn();
        }

        public void Respawn()
        {
            transform.position = respawnPos;
            rb.linearVelocity = Vector2.zero;

            moveInput = 0;
            dashInput = false;
            upInput = false;
        
            stateController.SetState(new IdleState(this, animator)); 
        }

        private void SetRespawnPos(Vector3 pos) => respawnPos = pos;
        public void SetMaxJumpCount(int newCount) => maxJumpCount = newCount;

        public void EnableAbility(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Run:
                    ableToRun = true;
                    break;
                case AbilityType.Jump:
                    ableToJump = true;
                    break;
                case AbilityType.Dash:
                    ableToDash = true;
                    break;
                case AbilityType.Interact:
                    ableToInteract = true;
                    break;
            }
        }


        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(jsonData);
            
            transform.position = saveData.playerPosition;
            respawnPos = saveData.playerRespawnPosition;
            ableToRun = saveData.ableToRun;
            ableToJump = saveData.ableToJump;
            ableToDash = saveData.ableToDash;
            ableToInteract = saveData.ableToInteract;
            maxJumpCount = saveData.maxJumpCount;
        }

        public string SaveData()
        {
            PlayerSaveData data = new()
            {
                playerPosition = transform.position,
                playerRespawnPosition = respawnPos,
                ableToRun = ableToRun,
                ableToJump = ableToJump,
                ableToDash = ableToDash,
                ableToInteract = ableToInteract,
                maxJumpCount = maxJumpCount
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}