using System;

namespace PSEMO.Events
{
    public static class UIEvents
    {
        public static event Action OnEndGame;
        public static void InvokeEndGame() => OnEndGame?.Invoke();

        //public static event Action OnLoadingStart;
        //public static void InvokeLoadingStart() => OnLoadingStart?.Invoke();
        //public static event Action OnLoadingEnd;
        //public static void InvokeLoadingEnd() => OnLoadingEnd?.Invoke();
        //public static event Action OnSavingStart;
        //public static void InvokeSavingStart() => OnSavingStart?.Invoke();
        //public static event Action OnSavingEnd;
        //public static void InvokeSavingEnd() => OnSavingEnd?.Invoke();
    }
}