using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //Change Anim
        //apply vertical velocity
    }

    public override void FixedUpdate()
    {
        //move
    }
}