using UnityEngine;

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
    }
}
