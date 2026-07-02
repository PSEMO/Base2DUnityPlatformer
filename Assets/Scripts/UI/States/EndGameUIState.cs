using UnityEngine;

namespace PSEMO.UI
{
    public class EndGameUIState : UIBaseState
    {
        public EndGameUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.GameEndBg,
            PanelType.EndGameMenu
        };
    }
}
