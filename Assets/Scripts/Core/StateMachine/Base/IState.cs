namespace PSEMO.Core.StateMachine
{
    public interface IState
    {
        void OnEnter(IState previousState);

        void Update();

        void FixedUpdate();
        void OnExit(IState nextState);
    }
}