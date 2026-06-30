using PSEMO.Core.Predicate;

namespace PSEMO.Core.StateMachine
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}