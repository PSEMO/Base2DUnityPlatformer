namespace PSEMO.UI
{
    public class MainMenuUIState : UIBaseState
    {
        public MainMenuUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.MainBg,
            PanelType.MainUI
        };

        protected override PanelType[] ActivePanels => _activePanels;
    }
}