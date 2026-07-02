using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace PSEMO.InputSettings
{
    public class RebindUI : MonoBehaviour
    {
        [Tooltip("The input action reference to rebind.")]
        [SerializeField] private InputActionReference inputActionReference;
        
        [Tooltip("Leave at -1 to automatically find the first binding, or specify a specific binding index (e.g. for composite parts like WASD).")]
        [SerializeField] private int bindingIndex = -1;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text actionNameText;
        [SerializeField] private TMP_Text bindingText;
        [SerializeField] private Button rebindButton;
        [SerializeField] private Button resetButton;

        [Header("State Objects (Optional)")]
        [SerializeField] private GameObject startRebindObject;
        [SerializeField] private GameObject waitingForInputObject;

        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        private void Start()
        {
            if (inputActionReference == null) return;
            
            // Automatically find index if not specified
            if (bindingIndex < 0)
            {
                bindingIndex = inputActionReference.action.GetBindingIndexForControl(inputActionReference.action.controls.FirstOrDefault());
                if (bindingIndex < 0) bindingIndex = 0; // Default fallback
            }

            if (actionNameText != null)
                actionNameText.text = inputActionReference.action.name;

            UpdateBindingDisplay();

            if (rebindButton != null)
                rebindButton.onClick.AddListener(StartRebinding);
                
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetBinding);
        }

        private void OnDestroy()
        {
            rebindingOperation?.Dispose();
        }

        private void StartRebinding()
        {
            if (inputActionReference == null) return;

            startRebindObject.SetActive(false);
            waitingForInputObject.SetActive(true);

            if (rebindButton != null) rebindButton.interactable = false;

            inputActionReference.action.Disable();

            rebindingOperation = inputActionReference.action.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete())
                .OnCancel(operation => RebindCanceled())
                .Start();
        }

        private void RebindComplete()
        {
            UpdateBindingDisplay();
            RebindManager.SaveOverrides(inputActionReference.asset);
            CleanUp();
        }

        private void RebindCanceled()
        {
            CleanUp();
        }

        private void ResetBinding()
        {
            if (inputActionReference == null) return;
            
            inputActionReference.action.RemoveBindingOverride(bindingIndex);
            RebindManager.SaveOverrides(inputActionReference.asset);
            UpdateBindingDisplay();
        }

        private void CleanUp()
        {
            rebindingOperation?.Dispose();
            rebindingOperation = null;

            startRebindObject.SetActive(true);
            waitingForInputObject.SetActive(false);

            if (rebindButton != null) rebindButton.interactable = true;

            inputActionReference.action.Enable();
        }

        private void UpdateBindingDisplay()
        {
            if (bindingText != null && inputActionReference != null)
            {
                bindingText.text = InputControlPath.ToHumanReadableString(
                    inputActionReference.action.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }
    }
}
