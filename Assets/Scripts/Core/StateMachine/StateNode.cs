using System.Collections.Generic;
using PSEMO.Core.Predicate;

namespace PSEMO.Core.StateMachine
{
    public class StateNode
    {
        public IState State { get; }
        public List<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}