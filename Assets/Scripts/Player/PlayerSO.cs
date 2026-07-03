using UnityEngine;

namespace PSEMO.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player")]
    public class PlayerSO : ScriptableObject
    {
        [Header("Camera")]
        public float camWeight = 1;

        [Header("Movement")]
        public float speed = 8f;

        [Header("Jump")]
        public int maxJumpCount = 3;
        public float jumpForce = 14f;
        
        [Space]

        public bool variableJump = true;
        [Range(0, 1)] public float variableJumpMult = 0;
        
        [Space]

        [Header("Dash")]
        public float dashForce = 40f;
        public float dashDuration = 0.12f;
        
        [Space]

        [Header("AbleTo")]
        public bool ableToRun = true;
        public bool ableToJump = true;
        public bool ableToDash = true;
        public bool ableToInteract = true;
        
        [Space]

        [Header("Config")]
        public float coyoteTime = 0.08f;
        public float jumpBufferTime = 0.12f;
        
        [Space]
        
        public float groundCheckDistance = 0.02f;
        public LayerMask groundLayer = 128;

        [Space]

        public float wallCheckDistance = 0.2f;
        public LayerMask wallLayer = 128;
    
        [Space]

        public float interactionRadius = 4f;
        public LayerMask interactionLayer = 64;
    }
}