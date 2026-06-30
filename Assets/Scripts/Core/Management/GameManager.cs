//TODO: REWORK

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

        CurrentGameState = initialGameState;
    }

    [HideInInspector] public GameState CurrentGameState { get; private set; }

    [SerializeField] private GameState initialGameState = GameState.MainMenu;

    public void UpdateGameState(GameState newState)
    {
        if (CurrentGameState != newState)
        {
            CurrentGameState = newState;
            Events.InvokeGameStateChanged(newState);
        }
    }
}