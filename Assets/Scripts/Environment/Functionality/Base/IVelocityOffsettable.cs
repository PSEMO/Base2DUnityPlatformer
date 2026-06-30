using PSEMO.Environment.Movement;

namespace PSEMO.Environment.Functionality
{
    public interface IVelocityOffsettable
    {
        void AddVelocityOffset(IMover offsetter);
        void RemoveVelocityOffset(IMover offsetter);
    }
}