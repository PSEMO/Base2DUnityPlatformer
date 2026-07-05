using System.Collections;
using UnityEngine;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class UIStateMachineController
    {
        private UIManager uiManager;
        public StateMachine UIStateMachine { get; private set; }

        private SignalPredicate settingsSignal = new();
        private SignalPredicate creditsSignal = new();
        private SignalPredicate backSignal = new();
        
        private IState previousUIState;

        private MainMenuUIState mainMenuUIState;
        private InGameUIState inGameUIState;
        private EndGameUIState endGameUIState;
        private LoadingUIState loadingUIState;

        public UIStateMachineController(UIManager manager, InitialPanel initialPanel)
        {
            uiManager = manager;
            InitializeStateMachine();
            SceneTypeToPanelState(initialPanel);
        }

        public void Update()
        {
            UIStateMachine.Update();
        }

        public void FixedUpdate()
        {
            UIStateMachine.FixedUpdate();
        }

        public void OnEnable()
        {
            UIEvents.OnEndGame += HandleEndGameSignal;
            UIEvents.OnLoadingStart += HandleLoadingStart;
            UIEvents.OnLoadingEnd += HandleLoadingEnd;

            UIEvents.OnBackClicked += backSignal.Fire;
            UIEvents.OnSettingsClicked += settingsSignal.Fire;
            UIEvents.OnCreditsClicked += creditsSignal.Fire;
        }

        public void OnDisable()
        {
            UIEvents.OnEndGame -= HandleEndGameSignal;
            UIEvents.OnLoadingStart -= HandleLoadingStart;
            UIEvents.OnLoadingEnd -= HandleLoadingEnd;

            UIEvents.OnBackClicked -= backSignal.Fire;
            UIEvents.OnSettingsClicked -= settingsSignal.Fire;
            UIEvents.OnCreditsClicked -= creditsSignal.Fire;
        }

        public void ProcessInputBack()
        {
            if (UIStateMachine.CurrentState is UIBaseState uiState)
            {
                uiState.OnBackRequested();
            }
        }

        public void ProcessInputNext()
        {
            if (UIStateMachine.CurrentState is UIBaseState uiState)
            {
                uiState.OnNextRequested();
            }
        }

        private void ForceSetState(UIBaseState state)
        {
            var previous = UIStateMachine.CurrentState as UIBaseState;
            UIStateMachine.SetState(state);
            
            if (previous?.GetActivePanels() != null)
            {
                foreach (var panelType in previous.GetActivePanels())
                {
                    uiManager.GetPanel(panelType).HideInstant();
                }
            }

            if (state?.GetActivePanels() != null)
            {
                foreach (var panelType in state.GetActivePanels())
                {
                    uiManager.GetPanel(panelType).ShowInstant();
                }
            }
        }

        private void HandleLoadingStart()
        {
            previousUIState = UIStateMachine.CurrentState;
            ForceSetState(loadingUIState);
        }

        private void HandleLoadingEnd()
        {
            uiManager.StartCoroutine(LoadingEndCoroutine(uiManager.UIData.extraDelayForLoading));
        }

        private IEnumerator LoadingEndCoroutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            
            UIStateMachine.SetState(previousUIState as UIBaseState);
        }

        private void HandleEndGameSignal()
        {
            UIStateMachine.SetState(endGameUIState);
        }

        private void SceneTypeToPanelState(InitialPanel state)
        {
            if (state == InitialPanel.MainMenu)
                ForceSetState(mainMenuUIState);
            else if (state == InitialPanel.InGame)
                ForceSetState(inGameUIState);
            else if (state == InitialPanel.EndMenu)
                ForceSetState(endGameUIState);
        }

        private void InitializeStateMachine()
        {
            UIStateMachine = new StateMachine();

            mainMenuUIState = new MainMenuUIState(uiManager);
            inGameUIState = new InGameUIState(uiManager);
            endGameUIState = new EndGameUIState(uiManager);
            loadingUIState = new LoadingUIState(uiManager);
            
            var mainSettingsUIState = new MainSettingsUIState(uiManager);
            var inGameSettingsUIState = new InGameSettingsUIState(uiManager);
            var creditsUIState = new CreditsUIState(uiManager);
            var inGameUnPausingUIState = new InGameUnPausingUIState(uiManager);

            void At(IState from, IState to, IPredicate condition) =>
                UIStateMachine.AddTransition(from, to, condition);

            IPredicate Or(params IPredicate[] predicates) =>
                new OrPredicate(predicates);

            At(mainMenuUIState, mainSettingsUIState, Or(settingsSignal, uiManager.InputBackSignal));
            At(mainMenuUIState, creditsUIState, creditsSignal);
        
            At(mainSettingsUIState, mainMenuUIState, Or(backSignal, uiManager.InputBackSignal));
        
            At(creditsUIState, mainMenuUIState, Or(backSignal, uiManager.InputBackSignal));

            At(inGameUIState, inGameSettingsUIState, Or(settingsSignal, uiManager.InputBackSignal));
        
            At(inGameSettingsUIState, inGameUnPausingUIState, Or(backSignal, uiManager.InputBackSignal));

            At(inGameUnPausingUIState, inGameUIState, new FuncPredicate(() => inGameUnPausingUIState.IsTimerComplete));
        }
    }
}