using PSEMO.Events;
using UnityEngine;

namespace PSEMO.UI
{
    public class InGameUnPausingUIState : UIBaseState
    {
        private float timer;

        public InGameUnPausingUIState(UIManager ctx) : base(ctx) {}

        private static readonly PanelType[] _activePanels = new[]
        {
            PanelType.InGameBg,
            PanelType.InGameUnPausingMenu
        };

        protected override PanelType[] ActivePanels => _activePanels;

        public override void OnEnter()
        {
            base.OnEnter();
            timer = 0f;
        }

        public override void Update()
        {
            base.Update();
            timer += Time.unscaledDeltaTime;
        }

        public override void OnExit()
        {
            base.OnExit();
            UIEvents.InvokeGameUnpause();
        }

        public bool IsTimerComplete => timer >= ctx.UIData.returningFromPauseCooldown;
    }
}