using UnityEngine;

namespace PSEMO.Core.StateMachine
{
    public abstract class BaseState<AnyController> : IState where AnyController : IStateMachineUser
    {
        protected readonly AnyController ctx;
        protected readonly Animator animator;

        protected BaseState(AnyController _ctx, Animator _animator)
        {
            ctx = _ctx;
            animator = _animator;
        }

        protected BaseState(AnyController _ctx)
        {
            ctx = _ctx;
            animator = null;
        }

        public virtual void OnEnter(IState previousState)
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit(IState nextState)
        {
            // noop
        }
    }
}