using PSEMO.Audio;
using PSEMO.Core.StateMachine;
using UnityEngine;

namespace PSEMO.Player
{
    public class FallState : PlayerBaseState
    {
        public FallState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter()
        {
            animator.Play(FallAnimHash);

            if(ctx.jumpsLeft == ctx.data.maxJumpCount)
            {
                ctx.jumpsLeft--;
            }
        }

        public override void FixedUpdate()
        {
            ctx.Run();
        }

        public override void OnExit(IState nextState)
        {
            if (nextState is RunState || nextState is IdleState)
            {
                AudioManager.Instance.PlayAudio(AudioConstants.Fall);
            }

            base.OnExit(nextState);
        }
    }
}