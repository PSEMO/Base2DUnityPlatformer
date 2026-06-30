namespace PSEMO.Core
{
    public interface IPoolable
    {
        public abstract string ID { get; set; }

        public abstract void ResetObject();
    }
}