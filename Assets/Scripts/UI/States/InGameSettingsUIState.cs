using PSEMO.Core.StateMachine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public class InGameSettingsUIState : UIBaseState
    {
        public InGameSettingsUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.InGameBg,
            PanelType.InGameSettings
        };

        protected override PanelType[] ActivePanels => _activePanels;

        public override bool IsMenuPanel => true;

        public override void OnEnter(IState nextState)
        {
            base.OnEnter(nextState);
            UIEvents.InvokeGamePause();
        }
    }
}