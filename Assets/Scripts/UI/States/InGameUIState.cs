namespace PSEMO.UI
{
    public class InGameUIState : UIBaseState
    {
        public InGameUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.InGameUI
        };
    }
}