using UnityEngine;

public class RunState : BaseState
{
    public RunState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //animator.Play(RunAnimHash);
    }

    public override void FixedUpdate()
    {
        ctx.Run();
    }
}