using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEngine.InputSystem.Editor;
#endif

using UnityEngine.InputSystem.UI;

namespace PSEMO.Input
{
    public class VirtualMouseDIY : MonoBehaviour
    {
        public RectTransform cursorTransform
        {
            get => m_CursorTransform;
            set => m_CursorTransform = value;
        }
        
        public float cursorSpeed
        {
            get => m_CursorSpeed;
            set => m_CursorSpeed = value;
        }
        
        public CursorMode cursorMode
        {
            get => m_CursorMode;
            set
            {
                if (m_CursorMode == value)
                    return;

                // If we're turning it off, make sure we re-enable the system mouse.
                if (m_CursorMode == CursorMode.HardwareCursorIfAvailable && m_SystemMouse != null)
                {
                    InputSystem.EnableDevice(m_SystemMouse);
                    m_SystemMouse = null;
                }

                m_CursorMode = value;

                if (m_CursorMode == CursorMode.HardwareCursorIfAvailable)
                    TryEnableHardwareCursor();
                else if (m_CursorGraphic != null)
                    m_CursorGraphic.enabled = true;
            }
        }
        
        public Graphic cursorGraphic
        {
            get => m_CursorGraphic;
            set
            {
                m_CursorGraphic = value;
                TryFindCanvas();
            }
        }

        public float scrollSpeed
        {
            get => m_ScrollSpeed;
            set => m_ScrollSpeed = value;
        }
        
        public Mouse virtualMouse => m_VirtualMouse;

        public InputActionProperty stickAction
        {
            get => m_StickAction;
            set => SetAction(ref m_StickAction, value);
        }
        
        public InputActionProperty leftButtonAction
        {
            get => m_LeftButtonAction;
            set
            {
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_LeftButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetAction(ref m_LeftButtonAction, value);
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_LeftButtonAction, m_ButtonActionTriggeredDelegate, true);
            }
        }
        
        public InputActionProperty rightButtonAction
        {
            get => m_RightButtonAction;
            set
            {
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_RightButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetAction(ref m_RightButtonAction, value);
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_RightButtonAction, m_ButtonActionTriggeredDelegate, true);
            }
        }
        
        public InputActionProperty middleButtonAction
        {
            get => m_MiddleButtonAction;
            set
            {
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_MiddleButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetAction(ref m_MiddleButtonAction, value);
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_MiddleButtonAction, m_ButtonActionTriggeredDelegate, true);
            }
        }
        
        public InputActionProperty forwardButtonAction
        {
            get => m_ForwardButtonAction;
            set
            {
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_ForwardButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetAction(ref m_ForwardButtonAction, value);
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_ForwardButtonAction, m_ButtonActionTriggeredDelegate, true);
            }
        }
        
        public InputActionProperty backButtonAction
        {
            get => m_BackButtonAction;
            set
            {
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_BackButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetAction(ref m_BackButtonAction, value);
                if (m_ButtonActionTriggeredDelegate != null)
                    SetActionCallback(m_BackButtonAction, m_ButtonActionTriggeredDelegate, true);
            }
        }

        public InputActionProperty scrollWheelAction
        {
            get => m_ScrollWheelAction;
            set => SetAction(ref m_ScrollWheelAction, value);
        }

        protected void OnEnable()
        {
            // Add mouse device.
            if (m_VirtualMouse == null)
            {
                m_VirtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
                TryFindCanvas();
            }
            
            // Hijack system mouse, if enabled.
            if (m_CursorMode == CursorMode.HardwareCursorIfAvailable)
                TryEnableHardwareCursor();

            else if (!m_VirtualMouse.added)
                InputSystem.AddDevice(m_VirtualMouse);

            // Set initial cursor position.
            if (m_CursorTransform != null)
            {
                var position = m_CursorTransform.anchoredPosition;
                InputState.Change(m_VirtualMouse.position, position);
                m_SystemMouse?.WarpCursorPosition(position);
            }

            // Hook into input update.
            if (m_AfterInputUpdateDelegate == null)
                m_AfterInputUpdateDelegate = OnAfterInputUpdate;
            InputSystem.onAfterUpdate += m_AfterInputUpdateDelegate;

            // Hook into actions.
            if (m_ButtonActionTriggeredDelegate == null)
                m_ButtonActionTriggeredDelegate = OnButtonActionTriggered;
            SetActionCallback(m_LeftButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(m_RightButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(m_MiddleButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(m_ForwardButtonAction, m_ButtonActionTriggeredDelegate, true);
            SetActionCallback(m_BackButtonAction, m_ButtonActionTriggeredDelegate, true);

            // Enable actions.
            m_StickAction.action?.Enable();
            m_LeftButtonAction.action?.Enable();
            m_RightButtonAction.action?.Enable();
            m_MiddleButtonAction.action?.Enable();
            m_ForwardButtonAction.action?.Enable();
            m_BackButtonAction.action?.Enable();
            m_ScrollWheelAction.action?.Enable();
        }

        protected void OnDisable()
        {
            // Let go of system mouse.
            if (m_SystemMouse != null)
            {
                InputSystem.EnableDevice(m_SystemMouse);
                m_SystemMouse = null;
            }

            // Remove mouse device.
            if (m_VirtualMouse != null && m_VirtualMouse.added)
                InputSystem.DisableDevice(m_VirtualMouse);

            // Remove ourselves from input update.
            if (m_AfterInputUpdateDelegate != null)
                InputSystem.onAfterUpdate -= m_AfterInputUpdateDelegate;

            // Disable actions.
            m_StickAction.action?.Disable();
            m_LeftButtonAction.action?.Disable();
            m_RightButtonAction.action?.Disable();
            m_MiddleButtonAction.action?.Disable();
            m_ForwardButtonAction.action?.Disable();
            m_BackButtonAction.action?.Disable();
            m_ScrollWheelAction.action?.Disable();

            // Unhock from actions.
            if (m_ButtonActionTriggeredDelegate != null)
            {
                SetActionCallback(m_LeftButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(m_RightButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(m_MiddleButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(m_ForwardButtonAction, m_ButtonActionTriggeredDelegate, false);
                SetActionCallback(m_BackButtonAction, m_ButtonActionTriggeredDelegate, false);
            }

            m_LastTime = default;
            m_LastStickValue = default;
        }

        private void TryFindCanvas()
        {
            m_Canvas = m_CursorGraphic?.GetComponentInParent<Canvas>();
            m_CanvasScaler =  m_CursorGraphic?.GetComponentInParent<CanvasScaler>();
        }

        private void TryEnableHardwareCursor()
        {
            var devices = InputSystem.devices;
            for (var i = 0; i < devices.Count; ++i)
            {
                var device = devices[i];
                if (device.native && device is Mouse mouse)
                {
                    m_SystemMouse = mouse;
                    break;
                }
            }

            if (m_SystemMouse == null)
            {
                if (m_CursorGraphic != null)
                    m_CursorGraphic.enabled = true;
                return;
            }

            InputSystem.DisableDevice(m_SystemMouse);

            // Sync position.
            if (m_VirtualMouse != null)
                m_SystemMouse.WarpCursorPosition(m_VirtualMouse.position.value);

            // Turn off mouse cursor image.
            if (m_CursorGraphic != null)
                m_CursorGraphic.enabled = false;
        }

        private void UpdateMotion()
        {
            if (m_VirtualMouse == null)
                return;

            // Read current stick value.
            var stickAction = m_StickAction.action;
            if (stickAction == null)
                return;
            var stickValue = stickAction.ReadValue<Vector2>();
            if (Mathf.Approximately(0, stickValue.x) && Mathf.Approximately(0, stickValue.y))
            {
                // Motion has stopped.
                m_LastTime = default;
                m_LastStickValue = default;
            }
            else
            {
                var resolutionXScale = 1f;
                var resolutionYScale = 1f;
                if (m_CanvasScaler != null)
                {
                    resolutionXScale = m_Canvas.pixelRect.xMax / m_CanvasScaler.referenceResolution.x;
                    resolutionYScale = m_Canvas.pixelRect.yMax / m_CanvasScaler.referenceResolution.y;
                }

                var currentTime = InputState.currentTime;
                if (Mathf.Approximately(0, m_LastStickValue.x) && Mathf.Approximately(0, m_LastStickValue.y))
                {
                    // Motion has started.
                    m_LastTime = currentTime;
                }

                // Compute delta.
                var deltaTime = (float)(currentTime - m_LastTime);
                var delta = new Vector2(m_CursorSpeed * resolutionXScale * stickValue.x * deltaTime, m_CursorSpeed * resolutionYScale * stickValue.y * deltaTime);

                // Update position.
                var currentPosition = m_VirtualMouse.position.value;
                var newPosition = currentPosition + delta;

                ////REVIEW: for the hardware cursor, clamp to something else?
                // Clamp to canvas.
                if (m_Canvas != null)
                {
                    // Clamp to canvas.
                    var pixelRect = m_Canvas.pixelRect;
                    newPosition.x = Mathf.Clamp(newPosition.x, pixelRect.xMin, pixelRect.xMax);
                    newPosition.y = Mathf.Clamp(newPosition.y, pixelRect.yMin, pixelRect.yMax);
                }

                ////REVIEW: the fact we have no events on these means that actions won't have an event ID to go by; problem?
                InputState.Change(m_VirtualMouse.position, newPosition);
                InputState.Change(m_VirtualMouse.delta, delta);

                // Update software cursor transform, if any.
                if (m_CursorTransform != null &&
                    (m_CursorMode == CursorMode.SoftwareCursor ||
                     (m_CursorMode == CursorMode.HardwareCursorIfAvailable && m_SystemMouse == null)))
                {
                    m_CursorTransform.anchoredPosition = m_CanvasScaler == null ? newPosition : new Vector2(newPosition.x / resolutionXScale, newPosition.y / resolutionYScale);
                }

                m_LastStickValue = stickValue;
                m_LastTime = currentTime;

                // Update hardware cursor.
                m_SystemMouse?.WarpCursorPosition(newPosition);
            }

            // Update scroll wheel.
            var scrollAction = m_ScrollWheelAction.action;
            if (scrollAction != null)
            {
                var scrollValue = scrollAction.ReadValue<Vector2>();
                scrollValue.x *= m_ScrollSpeed;
                scrollValue.y *= m_ScrollSpeed;

                InputState.Change(m_VirtualMouse.scroll, scrollValue);
            }
        }

        [Header("Cursor")]
        [Tooltip("Whether the component should set the cursor position of the hardware mouse cursor, if one is available. If so, "
            + "the software cursor pointed (to by 'Cursor Graphic') will be hidden.")]
        [SerializeField] private CursorMode m_CursorMode;
        [Tooltip("The graphic that represents the software cursor. This is hidden if a hardware cursor (see 'Cursor Mode') is used.")]
        [SerializeField] private Graphic m_CursorGraphic;
        [Tooltip("The transform for the software cursor. Will only be set if a software cursor is used (see 'Cursor Mode'). Moving the cursor "
            + "updates the anchored position of the transform.")]
        [SerializeField] private RectTransform m_CursorTransform;

        [Header("Motion")]
        [Tooltip("Speed in pixels per second with which to move the cursor. Scaled by the input from 'Stick Action'.")]
        [SerializeField] private float m_CursorSpeed = 400;
        [Tooltip("Scale factor to apply to 'Scroll Wheel Action' when setting the mouse 'scrollWheel' control.")]
        [SerializeField] private float m_ScrollSpeed = 45;

        [Space(10)]
        [Tooltip("Vector2 action that moves the cursor left/right (X) and up/down (Y) on screen.")]
        [SerializeField] private InputActionProperty m_StickAction;
        [Tooltip("Button action that triggers a left-click on the mouse.")]
        [SerializeField] private InputActionProperty m_LeftButtonAction;
        [Tooltip("Button action that triggers a middle-click on the mouse.")]
        [SerializeField] private InputActionProperty m_MiddleButtonAction;
        [Tooltip("Button action that triggers a right-click on the mouse.")]
        [SerializeField] private InputActionProperty m_RightButtonAction;
        [Tooltip("Button action that triggers a forward button (button #4) click on the mouse.")]
        [SerializeField] private InputActionProperty m_ForwardButtonAction;
        [Tooltip("Button action that triggers a back button (button #5) click on the mouse.")]
        [SerializeField] private InputActionProperty m_BackButtonAction;
        [Tooltip("Vector2 action that feeds into the mouse 'scrollWheel' action (scaled by 'Scroll Speed').")]
        [SerializeField] private InputActionProperty m_ScrollWheelAction;

        private Canvas m_Canvas; // Canvas that gives the motion range for the software cursor.
        private CanvasScaler m_CanvasScaler;
        private Mouse m_VirtualMouse;
        private Mouse m_SystemMouse;
        private Action m_AfterInputUpdateDelegate;
        private Action<InputAction.CallbackContext> m_ButtonActionTriggeredDelegate;
        private double m_LastTime;
        private Vector2 m_LastStickValue;

        private void OnButtonActionTriggered(InputAction.CallbackContext context)
        {
            if (m_VirtualMouse == null)
                return;

            // The button controls are bit controls. We can't (yet?) use InputState.Change to state
            // the change of those controls as the state update machinery of InputManager only supports
            // byte region updates. So we just grab the full state of our virtual mouse, then update
            // the button in there and then simply overwrite the entire state.

            var action = context.action;
            MouseButton? button = null;
            if (action == m_LeftButtonAction.action)
                button = MouseButton.Left;
            else if (action == m_RightButtonAction.action)
                button = MouseButton.Right;
            else if (action == m_MiddleButtonAction.action)
                button = MouseButton.Middle;
            else if (action == m_ForwardButtonAction.action)
                button = MouseButton.Forward;
            else if (action == m_BackButtonAction.action)
                button = MouseButton.Back;

            if (button != null)
            {
                var isPressed = context.control.IsPressed();
                m_VirtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(button.Value, isPressed);

                InputState.Change(m_VirtualMouse, mouseState);
            }
        }

        private static void SetActionCallback(InputActionProperty field, Action<InputAction.CallbackContext> callback, bool install = true)
        {
            var action = field.action;
            if (action == null)
                return;

            // We don't need the performed callback as our mouse buttons are binary and thus
            // we only care about started (1) and canceled (0).

            if (install)
            {
                action.started += callback;
                action.canceled += callback;
            }
            else
            {
                action.started -= callback;
                action.canceled -= callback;
            }
        }

        private static void SetAction(ref InputActionProperty field, InputActionProperty value)
        {
            var oldValue = field;
            field = value;

            if (oldValue.reference == null)
            {
                var oldAction = oldValue.action;
                if (oldAction != null && oldAction.enabled)
                {
                    oldAction.Disable();
                    if (value.reference == null)
                        value.action?.Enable();
                }
            }
        }

        private void OnAfterInputUpdate()
        {
            UpdateMotion();
        }

        public enum CursorMode
        {
            SoftwareCursor,
            HardwareCursorIfAvailable,
        }

    }
}