using System;
using UnityEngine;

namespace PSEMO.Events
{
    public static class UIEvents
    {
        public static event Action OnQuitToMainMenu;
        public static void InvokeQuit() => OnQuitToMainMenu?.Invoke();

        public static event Action OnEndGame;
        public static void InvokeEndGame() => OnEndGame?.Invoke();

        public static event Action OnGamePause;
        public static void InvokeGamePause() => OnGamePause?.Invoke();

        public static event Action OnGameUnpause;
        public static void InvokeGameUnpause() => OnGameUnpause?.Invoke();

        public static event Action OnBackClicked;
        public static void InvokeBack() => OnBackClicked?.Invoke();

        public static event Action OnSettingsClicked;
        public static void InvokeSettings() => OnSettingsClicked?.Invoke();

        public static event Action OnCreditsClicked;
        public static void InvokeCredits() => OnCreditsClicked?.Invoke();
        
        private static int activeLoadingCount = 0;
        private static int extraLoadingEndCount = 0;
        public static bool IsLoading => activeLoadingCount > 0;

        public static event Action OnLoadingStart;
        public static void InvokeLoadingStart() 
        {
            if (activeLoadingCount == 0)
            {
                OnLoadingStart?.Invoke();
            }
            activeLoadingCount++;
        }
        
        public static event Action OnLoadingEnd;
        public static void InvokeLoadingEnd()
        {
            if (activeLoadingCount > 0)
            {
                activeLoadingCount--;
                if (activeLoadingCount == 0)
                {
                    OnLoadingEnd?.Invoke();
                }
            }
            else
            {
                extraLoadingEndCount++;
                Debug.LogError($"InvokeLoadingEnd called when activeLoadingCount was already at: {activeLoadingCount}");
                Debug.LogError($"Loading was ended {extraLoadingEndCount} times more than it should have! ");
            }
        }
    }
}