using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject InGameMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject CreditsMenu;
    
    [SerializeField] GameObject BackGround;

    private void Start()
    {
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
        {
            SwitchToMainMenuUI();
        }
        else if (state == GameState.Playing)
        {
            SwitchToInGameMenuUI();
        }
    }

    public void SwitchToMainMenuUI()
    {
        DisableAllUI();
        SetBackgroundUI();

        MainMenu.SetActive(true);
    }

    public void SwitchToInGameMenuUI()
    {
        DisableAllUI();
        SetBackgroundUI();

        InGameMenu.SetActive(true);
    }

    public void SettingsBtn()
    {
        DisableAllUI();
        SetBackgroundUI();

        SettingsMenu.SetActive(true);
    }

    public void CreditsBtn()
    {
        DisableAllUI();
        SetBackgroundUI();

        CreditsMenu.SetActive(true);
    }

    public void BackBtn()
    {
        if (GameManager.Instance.currentGameState == GameState.MainMenu)
        {
            SwitchToMainMenuUI();
        }
        else if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            SwitchToInGameMenuUI();
        }
    }

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

    private void DisableAllUI()
    {
        MainMenu.SetActive(false);
        InGameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        BackGround.SetActive(false);
    }

    private void SetBackgroundUI()
    {
        if (GameManager.Instance.currentGameState == GameState.MainMenu)
        {
            BackGround.SetActive(true);
        }
        else if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            BackGround.SetActive(false);
        }
    }
}