using PSEMO.Core.StateMachine;
using UnityEngine;

namespace PSEMO.Player
{
    public class IdleState : PlayerBaseState
    {
        public IdleState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter(IState previousState)
        {
            animator.Play(IdleAnimHash);
        }

        public override void FixedUpdate()
        {
            ctx.rb.linearVelocity = new Vector2(0f, ctx.rb.linearVelocity.y);
        }
    }
}