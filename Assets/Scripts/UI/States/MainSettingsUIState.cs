namespace PSEMO.UI
{
    public class MainSettingsUIState : UIBaseState
    {
        public MainSettingsUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.MainBg,
            PanelType.MainSettings
        };

        protected override PanelType[] ActivePanels => _activePanels;

        public override bool IsMenuPanel => true;
    }
}