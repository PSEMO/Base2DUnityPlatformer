using UnityEngine;
using UnityEngine.UI;
using PSEMO.Core.Management;
using PSEMO.Core.Persistence;
using PSEMO.Events;
using System;

namespace PSEMO.UI
{
    public class UIGameActionHandler : MonoBehaviour
    {
        [SerializeField] private SceneSO SceneData;
        
        [Header("UI References")]
        [SerializeField] private Button ContinueBtnObj;

        [Serializable]
        public struct LevelButton
        {
            public Button button;
            public int sceneIndex;
        }

        [Header("Level Selection")]
        [SerializeField] private LevelButton[] levelButtons;

        private void Start()
        {
            UpdateContinueButton();
            UpdateLevelButtons();
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

        private void UpdateLevelButtons()
        {
            if (levelButtons.Length == 0)
                return;
                
            int furthestIndex = PersistenceManager.FurthestAvailableSceneIndex();
            int maxUnlockedIndex = Mathf.Max(furthestIndex, SceneData.firstGameSceneIndex);

            foreach (var lb in levelButtons)
            {
                lb.button.interactable = lb.sceneIndex <= maxUnlockedIndex;
            }
        }

        public void SelectSaveSlotBtn(string slotName)
        {
            PersistenceEvents.InvokeSaveSlotChanged(slotName);

            UpdateContinueButton();
            UpdateLevelButtons();
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

        public void SelectLevelBtn(int sceneIndex)
        {
            PersistenceEvents.InvokeSceneSaveDelete(sceneIndex);

            SceneLoader.Load(sceneIndex);
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
    }
}