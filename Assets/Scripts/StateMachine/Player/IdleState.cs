using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController _ctx, Animator _animator) : base(_ctx, _animator) { }

    public override void OnEnter()
    {
        //Change Anim
        //Remove horizontal velocity
    }
}