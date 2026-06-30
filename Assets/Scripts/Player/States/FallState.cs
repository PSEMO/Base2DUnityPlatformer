using UnityEngine;

namespace PSEMO.Player
{
    public class FallState : PlayerBaseState
    {
        public FallState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter()
        {
            //animator.Play(FallAnimHash);

            if(ctx.jumpsLeft == ctx.data.jumpCount)
            {
                ctx.jumpsLeft--;
            }
        }

        public override void FixedUpdate()
        {
            ctx.Run();
        }
    }
}
