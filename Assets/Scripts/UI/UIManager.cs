using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        panelDict = new Dictionary<PanelType, Panel>();

        foreach (var menu in panels)
        {
            if (menu != null && !panelDict.ContainsKey(menu.Type))
            {
                menu.HideInstant();
                panelDict.Add(menu.Type, menu);
            }
        }

        Events.OnGameStateChanged += HandleGameStateChanged;
    }

    [SerializeField] private List<Panel> panels; 
    private Dictionary<PanelType, Panel> panelDict;

    private void Start()
    {
        HandleGameStateChanged(GameManager.Instance.CurrentGameState);
    }

    private void OnDestroy()
    {
        Events.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
            SwitchToMainMenuUI();
        else if (state == GameState.Playing)
            SwitchToInGameMenuUI();
    }

    public void SwitchToMainMenuUI()
    {
        DisableAllUI();
        SetBg();

        panelDict[PanelType.MainUI].Show();
    }

    public void SwitchToInGameMenuUI()
    {
        DisableAllUI();
        SetBg();

        panelDict[PanelType.InGameUI].Show();
    }

    public void ToggleSettingMenu()
    {
        if (panelDict[PanelType.MainSettings].IsOpen || panelDict[PanelType.InGameSettings].IsOpen)
            BackBtn();
        else
            SettingsBtn();
    }

//#region Helper
    private void DisableAllUI()
    {
        foreach (Panel menuScreen in panelDict.Values)
        {
            menuScreen.Hide();
        }
    }

    private void SetBg()
    {
        if (GameManager.Instance.CurrentGameState == GameState.MainMenu)
            panelDict[PanelType.MainBg].Show();
    }
//#endregion

//#region Buttons

    public void PlayBtn()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);

        SceneManager.LoadScene(1);
    }

    public void QuitBtn()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
        
        SceneManager.LoadScene(0);
    }

    public void SettingsBtn()
    {
        DisableAllUI();
        SetBg();

        if (GameManager.Instance.CurrentGameState == GameState.MainMenu)
            panelDict[PanelType.MainSettings].Show();
        else if (GameManager.Instance.CurrentGameState == GameState.Playing)
            panelDict[PanelType.InGameSettings].Show();
    }

    public void CreditsBtn()
    {
        DisableAllUI();
        SetBg();

        panelDict[PanelType.CreditsMenu].Show();
    }

    public void BackBtn()
    {
        if (GameManager.Instance.CurrentGameState == GameState.MainMenu)
            SwitchToMainMenuUI();
        else if (GameManager.Instance.CurrentGameState == GameState.Playing)
            SwitchToInGameMenuUI();
    }
//#endregion
}