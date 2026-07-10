namespace PSEMO.UI
{
    public class LoadingUIState : UIBaseState
    {
        public LoadingUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.Loading,
            PanelType.LoadingBg
        };

        protected override PanelType[] ActivePanels => _activePanels;
    }
}
