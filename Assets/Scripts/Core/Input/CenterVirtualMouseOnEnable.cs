using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace PSEMO.Input
{
    public class CenterVirtualMouseOnEnable : MonoBehaviour
    {
        [Tooltip("The VirtualMouseDIY component. If left null, it will search in parent.")]
        [SerializeField] private VirtualMouseDIY virtualMouseDIY;
        
        RectTransform parentRect;
        UnityEngine.Camera mainCam;
        Canvas canvas;

        void Awake()
        {
            canvas = virtualMouseDIY.cursorGraphic.canvas;
            mainCam = canvas.worldCamera;
            parentRect = transform.parent as RectTransform;
        }

        private void OnEnable()
        {
            if (virtualMouseDIY.virtualMouse == null) return;
            
            Vector2 centerPos = canvas.pixelRect.center;

            InputState.Change(virtualMouseDIY.virtualMouse.position, centerPos);

            Mouse.current?.WarpCursorPosition(centerPos);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, centerPos, mainCam, out Vector2 localPoint))
            {
                virtualMouseDIY.cursorTransform.localPosition = localPoint;
            }
        }
    }
}
