using UnityEngine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class EndGameUIState : UIBaseState
    {
        public EndGameUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.GameEndBg,
            PanelType.EndGameMenu
        };

        protected override PanelType[] ActivePanels => _activePanels;

        public override void OnBackRequested()
        {
            UIEvents.InvokeQuit();
        }

        public override void OnNextRequested()
        {
            UIEvents.InvokeQuit();
        }
    }
}
