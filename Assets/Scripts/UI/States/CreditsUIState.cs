namespace PSEMO.UI
{
    public class CreditsUIState : UIBaseState
    {
        public CreditsUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.MainBg,
            PanelType.CreditsMenu
        };

        protected override PanelType[] ActivePanels => _activePanels;
    }
}