using System;

namespace PSEMO.Core.Predicate
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> _func)
        {
            func = _func;
        }

        public bool Evaluate() => func.Invoke();
    }
}