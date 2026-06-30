namespace PSEMO.UI
{
    public class MainMenuUIState : UIBaseState
    {
        public MainMenuUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.MainBg,
            PanelType.MainUI
        };
    }
}