using UnityEngine;
using UnityEngine.UI;
using PSEMO.Core.Management;
using PSEMO.Core.Persistence;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class UIGameActionHandler : MonoBehaviour
    {
        [SerializeField] private SceneSO SceneData;
        
        [Header("UI References")]
        [SerializeField] private Button ContinueBtnObj;

        private void Start()
        {
            UpdateContinueButton();
        }

        void OnEnable()
        {
            UIEvents.OnQuitToMainMenu += Quit;
        }

        void OnDisable()
        {
            UIEvents.OnQuitToMainMenu -= Quit;
        }

        private void UpdateContinueButton()
        {
            if (ContinueBtnObj != null)
                ContinueBtnObj.interactable = PersistenceManager.HasSceneData();
        }

        private void SelectSaveSlotBtn(string slotName)
        {
            PersistenceEvents.InvokeSaveSlotChanged(slotName);

            UpdateContinueButton();
        }

        private void Quit()
        {
            UIEvents.InvokeGameUnpause();

            SceneLoader.Load(SceneData.mainMenuSceneIndex);
        }

        public void ContinueBtn()
        {
            SceneLoader.Load(PersistenceManager.FurthestAvailableSceneIndex());
        }

        public void NewGameBtn()
        {
            PersistenceEvents.InvokeGameSaveDelete();

            SceneLoader.Load(SceneData.firstGameSceneIndex);
        }

        public void QuitBtn()
        {
            UIEvents.InvokeQuit();
        }

        public void QuitAndSaveBtn()
        {
            PersistenceEvents.InvokeGameSave();
            UIEvents.InvokeQuit();
        }

        public void BackBtn()
        {
            UIEvents.InvokeBack();
        }

        public void SettingsBtn()
        {
            UIEvents.InvokeSettings();
        }

        public void CreditsBtn()
        {
            UIEvents.InvokeCredits();
        }

        public void SaveBtn()
        {
            PersistenceEvents.InvokeGameSave();
        }

        public void SelectSaveSlot1Btn() => SelectSaveSlotBtn("SaveSlot1");
        public void SelectSaveSlot2Btn() => SelectSaveSlotBtn("SaveSlot2");
        public void SelectSaveSlot3Btn() => SelectSaveSlotBtn("SaveSlot3");
        public void SelectSaveSlot4Btn() => SelectSaveSlotBtn("SaveSlot4");
    }
}
