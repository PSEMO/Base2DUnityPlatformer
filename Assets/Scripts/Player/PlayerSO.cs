using UnityEngine;

namespace PSEMO.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player")]
    public class PlayerSO : ScriptableObject
    {
        [Header("Camera")]
        public float camDivisor = 1;

        [Header("Movement")]
        public float speed = 8f;

        [Header("Jump")]
        public int maxJumpCount = 3;
        public float jumpForce = 14f;
        public bool variableJump = true;
        [Range(0, 1)] public float variableJumpMult = 0;

        [Header("Dash")]
        public float dashForce = 40f;
        public float dashDuration = 0.12f;

        [Header("AbleTo")]
        public bool ableToRun = true;
        public bool ableToJump = true;
        public bool ableToDash = true;
        public bool ableToInteract = true;

        [Header("Config")]
        public float coyoteTime = 0.08f;
        public float jumpBufferTime = 0.12f;
        public float groundCheckDistance = 0.01f;
        public LayerMask groundLayer = 133;
    
        [Space]
        public float interactionRadius = 4f;
        public LayerMask interactionLayer = 64;
    }
}