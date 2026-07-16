using UnityEngine;
using UnityEngine.InputSystem;
using PSEMO.Environment.Functionality;

namespace PSEMO.Player
{
    public class PlayerInputHandler : InputSystem_Actions.IPlayerActions
    {
        private readonly PlayerController player;
        private readonly InputSystem_Actions inputActions;

        public PlayerInputHandler(PlayerController player)
        {
            this.player = player;
            inputActions = new InputSystem_Actions();
            Input.RebindManager.LoadOverrides(inputActions.asset);
            inputActions.Player.AddCallbacks(this);
        }

        public void OnEnable()
        {
            inputActions.Player.Enable();
            Events.UIEvents.OnGamePause += DisableInput;
            Events.UIEvents.OnGameUnpause += EnableInput;
        }
        
        public void OnDisable()
        {
            inputActions.Player.Disable();
            Events.UIEvents.OnGamePause -= DisableInput;
            Events.UIEvents.OnGameUnpause -= EnableInput;
        }

        private void DisableInput()
        {
            inputActions.Player.Disable();
            player.moveInput = 0;
            player.upInput = false;
            player.dashInput = false;
        }

        private void EnableInput()
        {
            inputActions.Player.Enable();
        }

        public void OnDestroy()
        {
            inputActions.Player.RemoveCallbacks(this);
            inputActions.Dispose();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (player.ableToRun)
            {
                player.moveInput = context.ReadValue<float>();
            }
        }

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed && player.ableToJump)
            {
                player.upInput = true;
                player.jumpBufferCounter = player.data.jumpBufferTime;
            }
            else if (context.canceled)
            {
                player.upInput = false;
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed && player.ableToDash)
            {
                player.dashInput = true;
            }
            else if (context.canceled)
            {
                player.dashInput = false;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && player.ableToInteract)
            {
                Collider2D hit = Physics2D.OverlapCircle(player.transform.position, player.data.interactionRadius, player.data.interactionLayer);

                if (hit != null && hit.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteracted();
                }
            }
        }
    }
}
