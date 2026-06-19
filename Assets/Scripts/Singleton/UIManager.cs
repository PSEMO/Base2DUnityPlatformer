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

    private void Start()
    {
        SwitchToMainMenuUI();
    }

    public void SwitchToMainMenuUI()
    {
        DisableAllUI();

        MainMenu.SetActive(true);
    }

    public void SwitchToInGameMenuUI()
    {
        DisableAllUI();

        InGameMenu.SetActive(true);
    }

    public void SettingsBtn()
    {
        DisableAllUI();

        SettingsMenu.SetActive(true);
    }

    public void CreditsBtn()
    {
        DisableAllUI();

        CreditsMenu.SetActive(true);
    }

    public void BackBtn()
    {
        if (GameManager.Instance.gameState == GameState.MainMenu)
        {
            SwitchToMainMenuUI();
        }
        else if (GameManager.Instance.gameState == GameState.Playing)
        {
            SwitchToInGameMenuUI();
        }
    }

    public void PlayBtn()
    {
        SwitchToInGameMenuUI();

        GameManager.Instance.gameState = GameState.Playing;

        SceneManager.LoadScene(1);
    }

    public void QuitBtn()
    {
        SwitchToMainMenuUI();

        GameManager.Instance.gameState = GameState.MainMenu;
        
        SceneManager.LoadScene(0);
    }

    private void DisableAllUI()
    {
        MainMenu.SetActive(false);
        InGameMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }
}