using UnityEngine;

namespace PSEMO.UI
{
    public class InGameSettingsUIState : UIBaseState
    {
        public InGameSettingsUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.InGameSettings
        };

        public override void OnEnter()
        {
            base.OnEnter();
            Time.timeScale = 0f;
        }
    }
}