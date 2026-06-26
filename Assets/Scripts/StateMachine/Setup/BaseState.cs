using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly PlayerController ctx;
    protected readonly Animator animator;

    protected static readonly int IdleAnimHash = Animator.StringToHash("Idle");
    protected static readonly int WalkAnimHash = Animator.StringToHash("Walk");
    protected static readonly int RunAnimHash = Animator.StringToHash("Run");
    protected static readonly int DashAnimHash = Animator.StringToHash("Dash");
    protected static readonly int JumpAnimHash = Animator.StringToHash("Jump");
    protected static readonly int FallAnimHash = Animator.StringToHash("Fall");

    protected BaseState(PlayerController _ctx, Animator _animator)
    {
        ctx = _ctx;
        animator = _animator;
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void OnEnter()
    {
        // noop
    }

    public virtual void OnExit()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }
}