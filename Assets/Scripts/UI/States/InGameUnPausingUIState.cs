using UnityEngine;

namespace PSEMO.UI
{
    public class InGameUnPausingUIState : UIBaseState
    {
        private float timer;

        public InGameUnPausingUIState(UIManager ctx) : base(ctx) {}

        protected override PanelType[] ActivePanels => new[]
        {
            PanelType.InGameBg,
            PanelType.InGameUnPausingMenu
        };

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
            Time.timeScale = 1f;
        }

        public bool IsTimerComplete => timer >= ctx.Data.returningFromPauseCooldown;
    }
}