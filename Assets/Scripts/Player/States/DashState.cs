using UnityEngine;

namespace PSEMO.Player
{
    public class DashState : PlayerBaseState
    {
        private float dashTimeLeft;
        private float originalGravity;
        private bool isDashing;

        public DashState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

        public override void OnEnter()
        {
            animator.Play(DashAnimHash);
        
            ctx.canDash = false;
            ctx.dashInput = false;
        
            dashTimeLeft = ctx.data.dashDuration;
            originalGravity = ctx.rb.gravityScale;
            ctx.rb.gravityScale = 0f;

            isDashing = true;

            ctx.rb.linearVelocity = new Vector2(ctx.facing * ctx.data.dashForce, 0f);
        }

        public override void Update()
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft < 0f)
            {
                isDashing = false;
            }
        }

        public override void FixedUpdate()
        {
            if (ctx.IsFacingWall())
            {
                isDashing = false;
                return;
            }

            if (isDashing)
            {
                ctx.rb.linearVelocity = new Vector2(ctx.facing * ctx.data.dashForce, 0f);
            }
        }

        public override void OnExit()
        {
            ctx.rb.gravityScale = originalGravity;
            ctx.rb.linearVelocity = new Vector2(0, 0); 
        
            isDashing = false;
        }

        public bool IsDashing() => isDashing;
    }
}