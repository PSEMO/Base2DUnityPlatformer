namespace PSEMO.UI
{
    public class CreditsUIState : UIBaseState
    {
        public CreditsUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.MainBg,
            PanelType.CreditsMenu
        };
    }
}