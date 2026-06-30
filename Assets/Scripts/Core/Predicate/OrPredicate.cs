namespace PSEMO.Core.Predicate
{
    public class OrPredicate : IPredicate
    {
        private readonly IPredicate[] predicates;

        public OrPredicate(params IPredicate[] predicates)
        {
            this.predicates = predicates;
        }

        public bool Evaluate()
        {
            bool anyTrue = false;
            foreach (var p in predicates)
            {
                if (p.Evaluate())
                    anyTrue = true;
            }

            return anyTrue;
        }
    }
}