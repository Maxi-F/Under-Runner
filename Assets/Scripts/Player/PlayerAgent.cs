using System.Collections.Generic;
using Events;
using FSM;
using Health;
using UnityEngine;

namespace Player
{
    public class PlayerAgent : Agent
    {
        [SerializeField] private HealthPoints healthPoints;

        [Header("Internal Events")]
        [SerializeField] private ActionEventsWrapper idleEvents;
        [SerializeField] private ActionEventsWrapper moveEvents;
        [SerializeField] private ActionEventsWrapper dashEvents;

        [Header("Initial Sequence Events")]
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onCinematicFinished;

        private State _idleState;
        private State _moveState;
        private State _dashState;

        protected override void OnEnable()
        {
            base.OnEnable();
            onCinematicStarted.onEvent.AddListener(DisableFSM);
            onCinematicFinished.onEvent.AddListener(EnableFSM);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onCinematicStarted.onEvent.RemoveListener(DisableFSM);
            onCinematicFinished.onEvent.RemoveListener(EnableFSM);
        }

        public void DisableFSM()
        {
            Fsm.Disable();
        }

        public void EnableFSM()
        {
            Fsm.Enable();
        }

        public void ChangeStateToMove()
        {
            Fsm.ChangeState(_moveState);
        }

        public void ChangeStateToDash()
        {
            Fsm.ChangeState(_dashState);
        }

        public void ChangeStateToIdle()
        {
            Fsm.ChangeState(_idleState);
        }

        protected override List<State> GetStates()
        {
            _idleState = new State();
            _idleState.EnterAction += idleEvents.ExecuteOnEnter;
            _idleState.UpdateAction += idleEvents.ExecuteOnUpdate;
            _idleState.ExitAction += idleEvents.ExecuteOnExit;

            _moveState = new State();
            _moveState.EnterAction += moveEvents.ExecuteOnEnter;
            _moveState.UpdateAction += moveEvents.ExecuteOnUpdate;
            _moveState.ExitAction += moveEvents.ExecuteOnExit;

            _dashState = new State();
            _dashState.EnterAction += dashEvents.ExecuteOnEnter;
            _dashState.UpdateAction += dashEvents.ExecuteOnUpdate;
            _dashState.ExitAction += dashEvents.ExecuteOnExit;

            Transition idleToMoveTransition = new Transition(_idleState, _moveState);
            _idleState.AddTransition(idleToMoveTransition);

            Transition moveToDashTransition = new Transition(_moveState, _dashState);
            _moveState.AddTransition(moveToDashTransition);

            Transition dashToMoveTransition = new Transition(_dashState, _moveState);
            _dashState.AddTransition(dashToMoveTransition);

            Transition dashToIdleTransition = new Transition(_dashState, _idleState);
            _dashState.AddTransition(dashToIdleTransition);

            return new List<State>()
            {
                _idleState,
                _moveState,
                _dashState
            };
        }
    }
}