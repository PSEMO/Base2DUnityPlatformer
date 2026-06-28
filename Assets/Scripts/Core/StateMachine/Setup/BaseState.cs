using UnityEngine;

public abstract class BaseState<AnyController> : IState where AnyController : IController
{
    protected readonly AnyController ctx;
    protected readonly Animator animator;

    protected BaseState(AnyController _ctx, Animator _animator)
    {
        ctx = _ctx;
        animator = _animator;
    }

    protected BaseState(AnyController _ctx)
    {
        ctx = _ctx;
        animator = null;
    }

    public virtual void OnEnter()
    {
        // noop
    }

    public virtual void Update()
    {
        // noop
    }

    public virtual void FixedUpdate()
    {
        // noop
    }

    public virtual void OnExit()
    {
        // noop
    }
}