namespace PSEMO.Core.Predicate
{
    public class SignalPredicate : IPredicate
    {
        bool isSignaled;

        public void Fire() => isSignaled = true;

        public void Reset() => isSignaled = false;

        public bool Evaluate()
        {
            if (isSignaled)
            {
                isSignaled = false;
                return true;
            }

            return false;
        }
    }
}