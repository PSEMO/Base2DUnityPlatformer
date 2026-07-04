using PSEMO.Core.StateMachine;

namespace PSEMO.UI
{
    public abstract class UIBaseState : BaseState<UIManager>
    {
        protected UIBaseState(UIManager ctx) : base(ctx) {}

        protected abstract PanelType[] ActivePanels { get; }

        public PanelType[] GetActivePanels() => ActivePanels;

        public override void OnEnter()
        {
            if (ActivePanels != null)
            {
                foreach (var type in ActivePanels)
                {
                    ctx.GetPanel(type)?.Show();
                }
            }
        }

        public override void OnExit()
        {
            if (ActivePanels != null)
            {
                foreach (var type in ActivePanels)
                {
                    ctx.GetPanel(type)?.Hide();
                }
            }
        }
    }
}