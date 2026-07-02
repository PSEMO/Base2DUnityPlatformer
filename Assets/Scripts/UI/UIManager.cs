using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.UI
{
    public class UIManager : MonoBehaviour, IStateMachineUser
    {
        public UISO Data;

        public StateMachine UIStateMachine { get; private set; }

        public SignalPredicate SettingsSignal { get; private set; } = new();
        public SignalPredicate CreditsSignal { get; private set; } = new();
        public SignalPredicate BackSignal { get; private set; } = new();
        public SignalPredicate InputBackSignal { get; private set; } = new();
        public SignalPredicate MainMenuStateSignal { get; private set; } = new();
        public SignalPredicate InGameStateSignal { get; private set; } = new();
        public SignalPredicate EndMenuStateSignal { get; private set; } = new();
        public SignalPredicate InputNextSignal { get; private set; } = new();

        [SerializeField] private List<Panel> panels; 
        private Dictionary<PanelType, Panel> panelDict;

        private InputSystem_Actions inputActions;

        [SerializeField] private SceneState initialSceneState = SceneState.MainMenuScene;
        public SceneState CurrentSceneState { get; private set; }

        [Space]
        public Button ContinueBtnObj;

        private void Awake()
        {
            panelDict = new();

            foreach (var menu in panels)
            {
                if (menu != null && !panelDict.ContainsKey(menu.Type))
                {
                    menu.HideInstant();
                    panelDict.Add(menu.Type, menu);
                }
            }

            inputActions = new InputSystem_Actions();

            InitializeStateMachine();

            CurrentSceneState = initialSceneState;
        }

        private void Start()
        {
            HandleSceneStateChanged(CurrentSceneState);

            ContinueBtnObj.interactable = PersistenceManager.HasSceneData();
        }

        private void Update()
        {
            UIStateMachine.Update();
        }

        private void FixedUpdate()
        {
            UIStateMachine.FixedUpdate();
        }

        private void OnEnable()
        {
            inputActions.Enable();
            inputActions.UI.Back.performed += OnInputBack;
            inputActions.UI.Next.performed += OnInputNext;
            UIEvents.OnEndGame += HandleEndGameSignal;
        }

        private void OnDisable()
        {
            inputActions.Disable();
            inputActions.UI.Back.performed -= OnInputBack;
            inputActions.UI.Next.performed -= OnInputNext;
            UIEvents.OnEndGame -= HandleEndGameSignal;
        }

        private void HandleEndGameSignal()
        {
            EndMenuStateSignal.Fire();
        }

        private void HandleSceneStateChanged(SceneState state)
        {
            if (state == SceneState.MainMenuScene)
                MainMenuStateSignal.Fire();
            else if (state == SceneState.GameScene)
                InGameStateSignal.Fire();
            else if (state == SceneState.EndMenuScene)
                EndMenuStateSignal.Fire();
        }

        private bool TryUpdateSceneState(SceneState to)
        {
            if (CurrentSceneState == to)
            {
                return false;
            }

            CurrentSceneState = to;
            HandleSceneStateChanged(to);
            return true;
        }
    
        public Panel GetPanel(PanelType type)
        {
            if (panelDict.TryGetValue(type, out var panel))
                return panel;
            return null;
        }

        //==== State Machine ======
        private void InitializeStateMachine()
        {
            UIStateMachine = new StateMachine();

            var mainMenuUIState = new MainMenuUIState(this);
            var inGameUIState = new InGameUIState(this);
            var mainSettingsUIState = new MainSettingsUIState(this);
            var inGameSettingsUIState = new InGameSettingsUIState(this);
            var creditsUIState = new CreditsUIState(this);
            var inGameUnPausingUIState = new InGameUnPausingUIState(this);
            var endGameUIState = new EndGameUIState(this);

            void At(IState from, IState to, IPredicate condition) =>
                UIStateMachine.AddTransition(from, to, condition);

            void Any(IState to, IPredicate condition) =>
                UIStateMachine.AddAnyTransition(to, condition);

            IPredicate Or(params IPredicate[] predicates) =>
                new OrPredicate(predicates);

            Any(mainMenuUIState, MainMenuStateSignal);
            Any(inGameUIState, InGameStateSignal);
            Any(endGameUIState, EndMenuStateSignal);

            At(mainMenuUIState, mainSettingsUIState, Or(SettingsSignal, InputBackSignal));
            At(mainMenuUIState, creditsUIState, CreditsSignal);
        
            At(mainSettingsUIState, mainMenuUIState, Or(BackSignal, InputBackSignal));
        
            At(creditsUIState, mainMenuUIState, Or(BackSignal, InputBackSignal));

            At(inGameUIState, inGameSettingsUIState, Or(SettingsSignal, InputBackSignal));
        
            At(inGameSettingsUIState, inGameUnPausingUIState, Or(BackSignal, InputBackSignal));
        
            At(inGameUnPausingUIState, inGameUIState, new FuncPredicate(() => inGameUnPausingUIState.IsTimerComplete));
        
            // Initial state doesn't matter because of HandleGameStateChanged signal.
            UIStateMachine.SetState(mainMenuUIState);
        }
        //=========================
    
        //==== Input/Button =======
        private void OnInputBack(InputAction.CallbackContext context)
        {
            if (UIStateMachine.CurrentState is EndGameUIState)
            {
                QuitBtn();
                return;
            }
            InputBackSignal.Fire();
        }

        private void OnInputNext(InputAction.CallbackContext context)
        {
            if (UIStateMachine.CurrentState is EndGameUIState)
            {
                QuitBtn();
                return;
            }
            InputNextSignal.Fire();
        }

        public void BackBtn()
        {
            BackSignal.Fire();
        }

        public void ContinueBtn()
        {
            TryUpdateSceneState(SceneState.GameScene);
            SceneManager.LoadScene(PersistenceManager.FurthestAvailableSceneIndex());
        }

        public void NewGameBtn()
        {
            PersistenceEvents.InvokeGameSaveDelete();
            TryUpdateSceneState(SceneState.GameScene);
            SceneManager.LoadScene(1);
        }

        public void QuitBtn()
        {
            TryUpdateSceneState(SceneState.MainMenuScene);
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void QuitAndSaveBtn()
        {
            TryUpdateSceneState(SceneState.MainMenuScene);
            Time.timeScale = 1;
            PersistenceEvents.InvokeGameSave();
            SceneManager.LoadScene(0);
        }

        public void SettingsBtn()
        {
            SettingsSignal.Fire();
        }

        public void CreditsBtn()
        {
            CreditsSignal.Fire();
        }

        public void SaveBtn()
        {
            PersistenceEvents.InvokeGameSave();
        }

        private void SelectSaveSlotBtn(string slotName)
        {
            PersistenceEvents.InvokeSaveSlotChanged(slotName);
            if (ContinueBtnObj != null)
            {
                ContinueBtnObj.interactable = PersistenceManager.HasSceneData();
            }
        }

        public void SelectSaveSlot1Btn() => SelectSaveSlotBtn("SaveSlot1");
        public void SelectSaveSlot2Btn() => SelectSaveSlotBtn("SaveSlot2");
        public void SelectSaveSlot3Btn() => SelectSaveSlotBtn("SaveSlot3");
        public void SelectSaveSlot4Btn() => SelectSaveSlotBtn("SaveSlot4");
        //=========================
    }
}