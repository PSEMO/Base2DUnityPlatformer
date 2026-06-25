using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player")]
public class PlayerSO : ScriptableObject
{
    [Header("Camera")]
    public float camDivisor = 1;

    [Header("Movement")]
    public float speed = 8f;
    public float sprintMultiplier = 1.2f;

    [Header("Jump")]
    public int jumpCount = 1;
    public float jumpForce = 14f;
    public bool variableJump = true;
    [Range(0, 1)] public float variableJumpMult = 0;

    [Header("Dash")]
    public float dashForce = 24f;
    public float dashDuration = 0.15f;

    [Header("Config")]
    public float coyoteTime = 0.08f;
    public float jumpBufferTime = 0.12f;
    public LayerMask groundLayer;
    public LayerMask interactionLayer;
    public float groundCheckDistance = 0.2f;
}