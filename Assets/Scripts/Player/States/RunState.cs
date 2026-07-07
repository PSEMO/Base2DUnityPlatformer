using PSEMO.Core.StateMachine;
using UnityEngine;

namespace PSEMO.Player
{
    public class RunState : PlayerBaseState
    {
        public RunState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter(IState previousState)
        {
            animator.Play(RunAnimHash);
        }

        public override void FixedUpdate()
        {
            ctx.Run();
        }
    }
}