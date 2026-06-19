using System.Runtime.InteropServices;
using UnityEngine;

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

    private void Start()
    {
        SwitchToMainMenuUI();
    }

    public void SwitchToMainMenuUI()
    {
        MainMenu.SetActive(true);
        InGameMenu.SetActive(false);
    }

    public void SwitchToInGameMenuUI()
    {
        MainMenu.SetActive(false);
        InGameMenu.SetActive(true);
    }
}