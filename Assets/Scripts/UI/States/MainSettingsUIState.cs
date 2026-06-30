namespace PSEMO.UI
{
    public class MainSettingsUIState : UIBaseState
    {
        public MainSettingsUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.MainBg,
            PanelType.MainSettings
        };
    }
}