namespace PSEMO.UI
{
    public class InGameUIState : UIBaseState
    {
        public InGameUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.InGameUI
        };

        protected override PanelType[] ActivePanels => _activePanels;
    }
}