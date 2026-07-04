using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Events;
using PSEMO.Core.Persistence;
using System.Collections;

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
        public SignalPredicate InputNextSignal { get; private set; } = new();

        private IState previousUIState;

        private MainMenuUIState mainMenuUIState;
        private InGameUIState inGameUIState;
        private EndGameUIState endGameUIState;
        private LoadingUIState loadingUIState;

        [SerializeField] private List<Panel> panels; 
        private Dictionary<PanelType, Panel> panelDict;

        private InputSystem_Actions inputActions;

        [SerializeField] private SceneState initialSceneState = SceneState.MainMenuScene;
        public SceneState CurrentSceneState { get; private set; }

        private Coroutine LoadEndCoroutine = null;

        [Space]
        public Button ContinueBtnObj;

        private void Awake()
        {
            panelDict = new();

            foreach (var panel in panels)
            {
                panel.HideInstant();
                panelDict.Add(panel.Type, panel);
            }

            inputActions = new InputSystem_Actions();
            InputSettings.RebindManager.LoadOverrides(inputActions.asset);

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
            UIEvents.OnLoadingStart += HandleLoadingStart;
            UIEvents.OnLoadingEnd += HandleLoadingEnd;
        }

        private void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
                inputActions.UI.Back.performed -= OnInputBack;
                inputActions.UI.Next.performed -= OnInputNext;
            }

            UIEvents.OnEndGame -= HandleEndGameSignal;
            UIEvents.OnLoadingStart -= HandleLoadingStart;
            UIEvents.OnLoadingEnd -= HandleLoadingEnd;
        }

        private void ForceSetState(UIBaseState state)
        {
            var previous = UIStateMachine.CurrentState as UIBaseState;
            UIStateMachine.SetState(state);
            
            if (previous?.GetActivePanels() != null)
            {
                foreach (var panelType in previous.GetActivePanels())
                {
                    GetPanel(panelType).HideInstant();
                }
            }

            if (state?.GetActivePanels() != null)
            {
                foreach (var panelType in state.GetActivePanels())
                {
                    GetPanel(panelType).ShowInstant();
                }
            }
        }

        private void HandleLoadingStart()
        {
            if (previousUIState != null) return;

            Time.timeScale = 0;
            previousUIState = UIStateMachine.CurrentState;
            ForceSetState(loadingUIState);
        }

        private void HandleLoadingEnd()
        {
            if (LoadEndCoroutine != null)
                StopCoroutine(LoadEndCoroutine);

            LoadEndCoroutine = StartCoroutine(LoadingEnd(Data.extraDelayForLoading));
        }

        IEnumerator LoadingEnd(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            
            if (previousUIState == null) yield break;

            Time.timeScale = 1;
            UIStateMachine.SetState(previousUIState as UIBaseState);
            previousUIState = null;
        }

        private void HandleEndGameSignal()
        {
            UIStateMachine.SetState(endGameUIState);
        }

        private void HandleSceneStateChanged(SceneState state)
        {
            if (state == SceneState.MainMenuScene)
                ForceSetState(mainMenuUIState);
            else if (state == SceneState.GameScene)
                ForceSetState(inGameUIState);
            else if (state == SceneState.EndMenuScene)
                ForceSetState(endGameUIState);
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

            mainMenuUIState = new MainMenuUIState(this);
            inGameUIState = new InGameUIState(this);
            var mainSettingsUIState = new MainSettingsUIState(this);
            var inGameSettingsUIState = new InGameSettingsUIState(this);
            var creditsUIState = new CreditsUIState(this);
            var inGameUnPausingUIState = new InGameUnPausingUIState(this);
            endGameUIState = new EndGameUIState(this);
            loadingUIState = new LoadingUIState(this);

            void At(IState from, IState to, IPredicate condition) =>
                UIStateMachine.AddTransition(from, to, condition);

            //void Any(IState to, IPredicate condition) =>
                //UIStateMachine.AddAnyTransition(to, condition);

            IPredicate Or(params IPredicate[] predicates) =>
                new OrPredicate(predicates);

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
            SceneManager.LoadScene(Data.firstGameSceneIndex);
        }

        public void QuitBtn()
        {
            TryUpdateSceneState(SceneState.MainMenuScene);
            Time.timeScale = 1;
            SceneManager.LoadScene(Data.mainMenuSceneIndex);
        }

        public void QuitAndSaveBtn()
        {
            TryUpdateSceneState(SceneState.MainMenuScene);
            Time.timeScale = 1;
            PersistenceEvents.InvokeGameSave();
            SceneManager.LoadScene(Data.mainMenuSceneIndex);
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
            ContinueBtnObj.interactable = PersistenceManager.HasSceneData();
        }

        public void SelectSaveSlot1Btn() => SelectSaveSlotBtn("SaveSlot1");
        public void SelectSaveSlot2Btn() => SelectSaveSlotBtn("SaveSlot2");
        public void SelectSaveSlot3Btn() => SelectSaveSlotBtn("SaveSlot3");
        public void SelectSaveSlot4Btn() => SelectSaveSlotBtn("SaveSlot4");
        //=========================
    }
}