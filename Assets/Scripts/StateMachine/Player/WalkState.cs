using UnityEngine;

public class WalkState : BaseState
{
    public WalkState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //Change Anim
    }

    public override void FixedUpdate()
    {
        //move
    }
}