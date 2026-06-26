using UnityEngine;

public class DashState : BaseState
{
    public DashState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //Change Anim
        //Add horizontal velocity
    }

    public override void FixedUpdate()
    {
        //Keep horizontal velocity
    }
}