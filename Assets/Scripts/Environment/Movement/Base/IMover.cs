using UnityEngine;

namespace PSEMO.Environment.Movement
{

    public interface IMover
    {
        public virtual Vector2 GetVelocity() => Vector2.zero;
    }
}