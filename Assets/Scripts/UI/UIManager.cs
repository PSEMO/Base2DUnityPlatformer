using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IStateMachineUser
{
    public UISO Data;

    public StateMachine UIStateMachine { get; private set; }

    public SignalPredicate SettingsSignal { get; private set; } = new SignalPredicate();
    public SignalPredicate CreditsSignal { get; private set; } = new SignalPredicate();
    public SignalPredicate BackSignal { get; private set; } = new SignalPredicate();
    public SignalPredicate InputBackSignal { get; private set; } = new SignalPredicate();
    public SignalPredicate MainMenuStateSignal { get; private set; } = new SignalPredicate();
    public SignalPredicate InGameStateSignal { get; private set; } = new SignalPredicate();

    [SerializeField] private List<Panel> panels; 
    private Dictionary<PanelType, Panel> panelDict;

    private InputSystem_Actions inputActions;

    [SerializeField] private SceneState initialSceneState = SceneState.MainMenuScene;
    public SceneState CurrentSceneState { get; private set; }

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
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Disable();
            inputActions.UI.Back.performed -= OnInputBack;
        }
    }

    private void HandleSceneStateChanged(SceneState state)
    {
        if (state == SceneState.MainMenuScene)
            MainMenuStateSignal.Fire();
        else if (state == SceneState.GameScene)
            InGameStateSignal.Fire();
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

        void At(IState from, IState to, IPredicate condition) =>
            UIStateMachine.AddTransition(from, to, condition);

        void Any(IState to, IPredicate condition) =>
            UIStateMachine.AddAnyTransition(to, condition);

        IPredicate Or(params IPredicate[] predicates) =>
            new OrPredicate(predicates);

        Any(mainMenuUIState, MainMenuStateSignal);
        Any(inGameUIState, InGameStateSignal);

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
        InputBackSignal.Fire();
    }

    public void BackBtn()
    {
        BackSignal.Fire();
    }

    public void PlayBtn()
    {
        TryUpdateSceneState(SceneState.GameScene);
        SceneManager.LoadScene(1);
    }

    public void QuitBtn()
    {
        TryUpdateSceneState(SceneState.MainMenuScene);
        Time.timeScale = 1;
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
    //=========================
}