using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

namespace PSEMO.InputSettings
{
    public class RebindUI : MonoBehaviour
    {
        [Tooltip("The input action reference to rebind.")]
        [SerializeField] private InputActionReference inputActionReference;
        
        [Tooltip("Specify a specific binding index")]
        [SerializeField] private int bindingIndex = -1;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text actionNameText;
        [SerializeField] private TMP_Text bindingText;
        [SerializeField] private Button rebindButton;
        [SerializeField] private Button resetButton;

        [Header("State Objects")]
        [SerializeField] private GameObject startRebindObject;
        [SerializeField] private GameObject waitingForInputObject;

        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        private void Start()
        {
            if (bindingIndex < 0)
            {
                Debug.LogError("invalid binding index");
                Destroy(this);
            }

            actionNameText.text = inputActionReference.action.name;

            UpdateBindingDisplay();

            rebindButton.onClick.AddListener(StartRebinding);
            
            resetButton.onClick.AddListener(ResetBinding);
        }

        private void OnDestroy()
        {
            rebindingOperation?.Dispose();
        }

        private void StartRebinding()
        {
            startRebindObject.SetActive(false);
            waitingForInputObject.SetActive(true);

            rebindButton.interactable = false;

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

            rebindButton.interactable = true;

            inputActionReference.action.Enable();
        }

        private void UpdateBindingDisplay()
        {
            bindingText.text = InputControlPath.ToHumanReadableString(
                inputActionReference.action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}
