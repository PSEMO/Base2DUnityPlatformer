using UnityEngine;

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

        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = ctx.TimeScaleData.pauseTimeScale;
        }
    }
}