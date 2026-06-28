using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //animator.Play(JumpAnimHash);
        
        ctx.rb.linearVelocity = new Vector2(ctx.rb.linearVelocity.x, ctx.data.jumpForce);
        ctx.jumpBufferCounter = 0f;
        ctx.coyoteTimeCounter = 0f;
        ctx.jumpsLeft--;
        ctx.hasJumped = true;
    }

    public override void FixedUpdate()
    {
        ctx.Run();

        if (!ctx.upInput && ctx.rb.linearVelocity.y > 0f && ctx.data.variableJump)
        {
            ctx.rb.linearVelocity = new Vector2(ctx.rb.linearVelocityX, ctx.rb.linearVelocityY * ctx.data.variableJumpMult);
        }
    }
}