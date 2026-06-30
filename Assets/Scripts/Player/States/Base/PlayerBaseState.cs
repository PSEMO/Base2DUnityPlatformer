using UnityEngine;
using PSEMO.Core.StateMachine;

namespace PSEMO.Player
{

    public abstract class PlayerBaseState : BaseState<PlayerController>
    {
        protected static readonly int IdleAnimHash = Animator.StringToHash("Idle");
        protected static readonly int RunAnimHash = Animator.StringToHash("Run");
        protected static readonly int DashAnimHash = Animator.StringToHash("Dash");
        protected static readonly int JumpAnimHash = Animator.StringToHash("Jump");
        protected static readonly int FallAnimHash = Animator.StringToHash("Fall");

        protected PlayerBaseState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator)
        {
        }
    }
}