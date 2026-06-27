using System;
using System.Collections.Generic;

public class StateMachine
{
    StateNode current;
    Dictionary<Type, StateNode> Nodes = new();
    HashSet<ITransition> AnyTransition = new();

    public IState CurrentState => current?.State;

    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
            ChangeState(transition.To);

        current?.State?.Update();
    }

    public void FixedUpdate()
    {
        current?.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        current?.State?.OnExit();
        current = Nodes[state.GetType()];
        current?.State?.OnEnter();
    }

    private void ChangeState(IState state)
    {
        if (state == current?.State) return;

        var previousState = current?.State;
        var nextState = Nodes[state.GetType()].State;

        previousState?.OnExit();
        nextState?.OnEnter();
        current = Nodes[state.GetType()];
    }

    ITransition GetTransition()
    {
        foreach (var transition in AnyTransition)
            if (transition.Condition.Evaluate())
                return transition;

        if (current != null)
        {
            foreach (var transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;
        }

        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        AnyTransition.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    StateNode GetOrAddNode(IState state)
    {
        var node = Nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            Nodes.Add(state.GetType(), node);
        }

        return node;
    }
}