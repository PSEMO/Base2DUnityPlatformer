using System;
using UnityEngine;
using PSEMO.Core.Predicate;
using PSEMO.Core.StateMachine;

namespace PSEMO.Player
{
    public class PlayerStateMachineController
    {
        private readonly StateMachine stateMachine;

        public PlayerStateMachineController(PlayerController player, Animator animator)
        {
            stateMachine = new StateMachine();

            var idleState = new IdleState(player, animator);
            var runState = new RunState(player, animator);
            var jumpState = new JumpState(player, animator);
            var fallState = new FallState(player, animator);
            var dashState = new DashState(player, animator);

            void At(IState from, IState to, Func<bool> condition) =>
                stateMachine.AddTransition(from, to, new FuncPredicate(condition));

            void Any(IState to, Func<bool> condition) =>
                stateMachine.AddAnyTransition(to, new FuncPredicate(condition));

            At(idleState, runState, () => Mathf.Abs(player.moveInput) >= 0.1f);

            At(runState, idleState, () => Mathf.Abs(player.moveInput) < 0.1f);

            At(fallState, idleState, () => player.isGrounded && Mathf.Abs(player.moveInput) < 0.1f);
            At(fallState, runState, () => player.isGrounded && Mathf.Abs(player.moveInput) >= 0.1f);
        
            At(dashState, idleState, () => !dashState.IsDashing() && player.isGrounded && Mathf.Abs(player.moveInput) < 0.1f);
            At(dashState, runState, () => !dashState.IsDashing() && player.isGrounded && Mathf.Abs(player.moveInput) >= 0.1f);
            At(dashState, fallState, () => !dashState.IsDashing() && !player.isGrounded);

            Any(jumpState, () => player.jumpBufferCounter > 0f && (player.coyoteTimeCounter > 0f || player.jumpsLeft > 0));

            Any(dashState, () => player.dashInput && player.canDash);

            Any(fallState, () => player.rb.linearVelocityY < -0.1f);

            stateMachine.SetState(idleState);
        }

        public void Update() => stateMachine.Update();
        public void FixedUpdate() => stateMachine.FixedUpdate();
        public void SetState(IState state) => stateMachine.SetState(state);
    }
}
