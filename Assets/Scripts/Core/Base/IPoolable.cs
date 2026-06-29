using System;

public interface IPoolable
{
    public abstract string ID { get; set; }

    public virtual string GetID => ID;

    public abstract void ResetObject();
}