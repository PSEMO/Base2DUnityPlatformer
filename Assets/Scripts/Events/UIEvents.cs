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
                Debug.LogWarning("InvokeLoadingEnd called when activeLoadingCount is already 0.");
            }
        }
    }
}