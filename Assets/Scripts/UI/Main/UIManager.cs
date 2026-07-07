using UnityEngine;
using UnityEngine.InputSystem;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Events;

namespace PSEMO.UI
{
    [RequireComponent(typeof(UIPanelRegistry))]
    public class UIManager : MonoBehaviour, IStateMachineUser
    {
        public UISO UIData;

        [SerializeField] private InitialPanel initialPanel = InitialPanel.MainMenu;

        private InputSystem_Actions inputActions;
        private UIPanelRegistry panelRegistry;
        
        private UIStateMachineController stateController;

        public SignalPredicate InputBackSignal { get; private set; } = new();
        public SignalPredicate InputNextSignal { get; private set; } = new();

        private void Awake()
        {
            panelRegistry = GetComponent<UIPanelRegistry>();

            inputActions = new InputSystem_Actions();
            InputSettings.RebindManager.LoadOverrides(inputActions.asset);

            stateController = new UIStateMachineController(this, initialPanel);
        }

        private void Update()
        {
            stateController.Update();
        }

        private void FixedUpdate()
        {
            stateController.FixedUpdate();
        }

        private void OnEnable()
        {
            inputActions.Enable();
            inputActions.UI.Back.performed += OnInputBack;
            inputActions.UI.Accept.performed += OnInputAccept;
            inputActions.UI.Right.performed += OnInputRight;
            inputActions.UI.Left.performed += OnInputLeft;

            stateController.OnEnable();
        }

        private void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.UI.Back.performed -= OnInputBack;
                inputActions.UI.Accept.performed -= OnInputAccept;
                inputActions.UI.Right.performed -= OnInputRight;
                inputActions.UI.Left.performed -= OnInputLeft;
            }

            stateController?.OnDisable();
        }

        private void OnInputBack(InputAction.CallbackContext context)
        {
            stateController.ProcessInputBack();
        }

        private void OnInputAccept(InputAction.CallbackContext context)
        {
            stateController.ProcessInputNext();
        }

        private void OnInputRight(InputAction.CallbackContext context)
        {
            UIEvents.InvokeInputRight();
        }

        private void OnInputLeft(InputAction.CallbackContext context)
        {
            UIEvents.InvokeInputLeft();
        }

        public Panel GetPanel(PanelType type) => panelRegistry.GetPanel(type);
    }
}