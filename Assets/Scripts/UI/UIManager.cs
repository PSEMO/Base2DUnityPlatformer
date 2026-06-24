using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private List<Panel> menuScreens; 
    private Dictionary<MenuType, Panel> menuDict;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        menuDict = new Dictionary<MenuType, Panel>();

        foreach (var menu in menuScreens)
        {
            if (menu != null && !menuDict.ContainsKey(menu.Type))
            {
                menu.Hide();
                menuDict.Add(menu.Type, menu);
            }
        }

        HandleGameStateChanged(GameManager.Instance.currentGameState);

        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
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

        menuDict[MenuType.MainUI].Show();
    }

    public void SwitchToInGameMenuUI()
    {
        DisableAllUI();
        SetBg();

        menuDict[MenuType.InGameUI].Show();
    }

//#region Helper
    private void DisableAllUI()
    {
        foreach (Panel menuScreen in menuDict.Values)
        {
            menuScreen.Hide();
        }
    }

    private void SetBg()
    {
        if (GameManager.Instance.currentGameState == GameState.MainMenu)
            menuDict[MenuType.MainBg].Show();
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

        if (GameManager.Instance.currentGameState == GameState.MainMenu)
            menuDict[MenuType.MainSettings].Show();
        else if (GameManager.Instance.currentGameState == GameState.Playing)
            menuDict[MenuType.InGameSettings].Show();
    }

    public void CreditsBtn()
    {
        DisableAllUI();
        SetBg();

        menuDict[MenuType.CreditsMenu].Show();
    }

    public void BackBtn()
    {
        if (GameManager.Instance.currentGameState == GameState.MainMenu)
            SwitchToMainMenuUI();
        else if (GameManager.Instance.currentGameState == GameState.Playing)
            SwitchToInGameMenuUI();
    }
//#endregion
}