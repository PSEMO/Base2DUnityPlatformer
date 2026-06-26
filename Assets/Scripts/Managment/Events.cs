using System;
using UnityEngine;

public static class Events
{
    // Define your static events here
    public static event Action OnPlayerDeath;
    public static void InvokePlayerDeath() => OnPlayerDeath?.Invoke();

    public static event Action<Vector3> OnCheckPointReached;
    public static void InvokeCheckPointReached(Vector3 newSpawnPos) => OnCheckPointReached?.Invoke(newSpawnPos);

    public static event Action<GameState> OnGameStateChanged;
    public static void InvokeGameStateChanged(GameState newState) => OnGameStateChanged?.Invoke(newState);
}
