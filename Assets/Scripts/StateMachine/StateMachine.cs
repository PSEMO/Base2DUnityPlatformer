using System;
using System.Collections.Generic;

public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> nodes = new();
    HashSet<ITransition> anyTransition = new();

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
            ChangeState(transition.To);

        current.State?.Update();
    }

    public void FixedUpdate()
    {
        current.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        current = nodes[state.GetType()];
        current.State?.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if (state == current.State) return;

        var previousState = current.State;
        var nextState = nodes[state.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();
        current = nodes[state.GetType()];
    }

    ITransition GetTransition()
    {
        foreach (var transition in anyTransition)
            if (transition.Condition.Evaluate())
                return transition;

        foreach (var transition in current.Transitions)
            if (transition.Condition.Evaluate())
                return transition;

        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransition.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
}