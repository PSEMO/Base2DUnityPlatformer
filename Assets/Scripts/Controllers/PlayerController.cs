using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] PlayerSO data;

    InputSystem_Actions inputActions;

    float moveInput;
    bool upInput;
    bool downInput;
    bool sprintInput;
    bool interactInput;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.AddCallbacks(this);
    }

    void Start()
    {
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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        upInput = context.performed;
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        downInput = context.performed;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprintInput = context.performed;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        interactInput = context.performed;
    }
}