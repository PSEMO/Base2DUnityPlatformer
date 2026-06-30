//TODO: REWORK

using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.Back.performed += ToggleSettingsMenu;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.UI.Back.performed -= ToggleSettingsMenu;
    }

    private void ToggleSettingsMenu(InputAction.CallbackContext context)
    {
        UIManager.Instance.ToggleSettingMenu();
    }
}