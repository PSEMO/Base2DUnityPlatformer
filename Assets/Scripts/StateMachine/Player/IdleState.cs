using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //animator.Play(IdleAnimHash);
    }

    public override void FixedUpdate()
    {
        ctx.rb.linearVelocity = new Vector2(0f, ctx.rb.linearVelocity.y);
    }
}