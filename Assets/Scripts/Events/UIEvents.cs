using System;

namespace PSEMO.Events
{
    public static class UIEvents
    {
        public static event Action OnEndGame;
        public static void InvokeEndGame() => OnEndGame?.Invoke();
    }
}