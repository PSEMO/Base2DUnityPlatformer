using System;
using System.Collections.Generic;
using PSEMO.Core.Predicate;

namespace PSEMO.Core.StateMachine
{
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

            current.State.Update();
        }

        public void FixedUpdate()
        {
            current.State.FixedUpdate();
        }

        public event Action<IState, IState> OnStateChanged;

        public void SetState(IState state)
        {
            GetOrAddNode(state);
            var previousState = current?.State;
            previousState?.OnExit(Nodes[state.GetType()].State);
            
            current = Nodes[state.GetType()];
            current.State.OnEnter(previousState);
            
            OnStateChanged?.Invoke(previousState, current.State);
        }

        private void ChangeState(IState state)
        {
            if (state == current?.State) return;

            var previousState = current?.State;
            var nextState = Nodes[state.GetType()].State;

            previousState?.OnExit(nextState);
            nextState.OnEnter(previousState);
            current = Nodes[state.GetType()];
            
            OnStateChanged?.Invoke(previousState, nextState);
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
}