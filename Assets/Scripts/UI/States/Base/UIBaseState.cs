using System.Linq;
using PSEMO.Core.StateMachine;
using PSEMO.Events;

namespace PSEMO.UI
{
    public abstract class UIBaseState : BaseState<UIManager>
    {
        protected UIBaseState(UIManager ctx) : base(ctx) {}

        protected abstract PanelType[] ActivePanels { get; }

        public PanelType[] GetActivePanels() => ActivePanels;

        public virtual bool IsMenuPanel => false;

        public override void OnEnter(IState previousState)
        {
            UIEvents.InvokeMenuVisibilityChanged(IsMenuPanel);
            
            if (previousState is UIBaseState prevUIState)
            {
                var prevPanels = prevUIState.GetActivePanels();

                foreach (var type in ActivePanels)
                {
                    if (prevPanels.Contains(type))
                        continue;

                    ctx.GetPanel(type).Show();
                }
            }
            else
            {
                foreach (var type in ActivePanels)
                {
                    ctx.GetPanel(type).Show();
                }
            }
        }

        public override void OnExit(IState nextState)
        {
            UIBaseState nextUIState = nextState as UIBaseState;
            var nextPanels = nextUIState.GetActivePanels();

            foreach (var type in ActivePanels)
            {
                if (nextPanels.Contains(type))
                    continue;

                ctx.GetPanel(type).Hide();
            }
        }

        public virtual void OnBackRequested()
        {
            ctx.InputBackSignal.Fire();
        }

        public virtual void OnNextRequested()
        {
            ctx.InputNextSignal.Fire();
        }
    }
}