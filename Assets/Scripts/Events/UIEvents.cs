using System;
using UnityEngine;

namespace PSEMO.Events
{
    public static class UIEvents
    {
        public static event Action OnEndGame;
        public static void InvokeEndGame() => OnEndGame?.Invoke();

        public static event Action OnLoadingStart;
        public static event Action OnLoadingEnd;
        
        private static int activeLoadingCount = 0;

        public static bool IsLoading => activeLoadingCount > 0;
    
#if UNITY_EDITOR
        private static int extraLoadingEndCount = 0;
#endif

        public static void InvokeLoadingStart() 
        {
            if (activeLoadingCount == 0)
            {
                OnLoadingStart?.Invoke();
            }
            activeLoadingCount++;
        }
        
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
#if UNITY_EDITOR
                extraLoadingEndCount++;
                Debug.LogError($"InvokeLoadingEnd called when activeLoadingCount was already at: {activeLoadingCount}");
                Debug.LogError($"Loading was ended {extraLoadingEndCount} times more than it should have! ");
#endif
            }
        }
    }
}