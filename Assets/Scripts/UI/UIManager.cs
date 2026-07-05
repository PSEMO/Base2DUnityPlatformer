using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;
using PSEMO.Events;

namespace PSEMO.UI
{
    [RequireComponent(typeof(UIPanelRegistry))]
    public class UIManager : MonoBehaviour, IStateMachineUser
    {
        public UISO UIData;

        [SerializeField] private SceneState initialSceneState = SceneState.MainMenuScene;
        [HideInInspector] public SceneState CurrentSceneState { get; private set; }

        private InputSystem_Actions inputActions;

        private UIPanelRegistry panelRegistry;

        private void Awake()
        {
            panelRegistry = GetComponent<UIPanelRegistry>();

            inputActions = new InputSystem_Actions();
            InputSettings.RebindManager.LoadOverrides(inputActions.asset);

            InitializeStateMachine();
            CurrentSceneState = initialSceneState;
        }

        private void Start()
        {
            HandleSceneStateChanged(CurrentSceneState);
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

            UIEvents.OnBackClicked += HandleBackClicked;
            UIEvents.OnSettingsClicked += HandleSettingsClicked;
            UIEvents.OnCreditsClicked += HandleCreditsClicked;
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

            UIEvents.OnBackClicked -= HandleBackClicked;
            UIEvents.OnSettingsClicked -= HandleSettingsClicked;
            UIEvents.OnCreditsClicked -= HandleCreditsClicked;
        }

        private void OnInputBack(InputAction.CallbackContext context)
        {
            if (UIStateMachine.CurrentState is EndGameUIState)
            {
                UIEvents.InvokeQuit();
                return;
            }
            
            InputBackSignal.Fire();
        }

        private void OnInputNext(InputAction.CallbackContext context)
        {
            if (UIStateMachine.CurrentState is EndGameUIState)
            {
                UIEvents.InvokeQuit();
                return;
            }

            InputNextSignal.Fire();
        }

        private void HandleBackClicked() => BackSignal.Fire();
        private void HandleSettingsClicked() => SettingsSignal.Fire();
        private void HandleCreditsClicked() => CreditsSignal.Fire();

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
            previousUIState = UIStateMachine.CurrentState;
            ForceSetState(loadingUIState);
        }

        private void HandleLoadingEnd()
        {
            StartCoroutine(LoadingEnd(UIData.extraDelayForLoading));
        }

        IEnumerator LoadingEnd(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            UIStateMachine.SetState(previousUIState as UIBaseState);
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
    
        public Panel GetPanel(PanelType type) => panelRegistry.GetPanel(type);

        //==== State Machine ======
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
        
        private void InitializeStateMachine()
        {
            UIStateMachine = new StateMachine();

            mainMenuUIState = new MainMenuUIState(this);
            inGameUIState = new InGameUIState(this);
            endGameUIState = new EndGameUIState(this);
            loadingUIState = new LoadingUIState(this);
            
            var mainSettingsUIState = new MainSettingsUIState(this);
            var inGameSettingsUIState = new InGameSettingsUIState(this);
            var creditsUIState = new CreditsUIState(this);
            var inGameUnPausingUIState = new InGameUnPausingUIState(this);

            void At(IState from, IState to, IPredicate condition) =>
                UIStateMachine.AddTransition(from, to, condition);

            IPredicate Or(params IPredicate[] predicates) =>
                new OrPredicate(predicates);

            At(mainMenuUIState, mainSettingsUIState, Or(SettingsSignal, InputBackSignal));
            At(mainMenuUIState, creditsUIState, CreditsSignal);
        
            At(mainSettingsUIState, mainMenuUIState, Or(BackSignal, InputBackSignal));
        
            At(creditsUIState, mainMenuUIState, Or(BackSignal, InputBackSignal));

            At(inGameUIState, inGameSettingsUIState, Or(SettingsSignal, InputBackSignal));
        
            At(inGameSettingsUIState, inGameUnPausingUIState, Or(BackSignal, InputBackSignal));
            
            At(inGameUnPausingUIState, inGameUIState, new FuncPredicate(() => inGameUnPausingUIState.IsTimerComplete));
        }
        //=========================
    }
}