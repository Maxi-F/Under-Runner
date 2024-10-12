using System.Collections;
using System.Collections.Generic;
using FSM;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAgent : Agent
{
    [SerializeField] private HealthPoints healthPoints;

    [Header("Internal Events")]
    [SerializeField] private ActionEventsWrapper idleEvents;
    [SerializeField] private ActionEventsWrapper moveEvents;
    [FormerlySerializedAs("attackEvents")]
    [SerializeField] private ActionEventsWrapper combatEvents;

    private State _idleState;
    private State _weakenedState;
    private State _combatState;

    public void ChangeStateToWeakened()
    {
        Fsm.ChangeState(_weakenedState);
    }

    public void ChangeStateToCombat()
    {
        Fsm.ChangeState(_combatState);
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

        _weakenedState = new State();
        _weakenedState.EnterAction += moveEvents.ExecuteOnEnter;
        _weakenedState.UpdateAction += moveEvents.ExecuteOnUpdate;
        _weakenedState.ExitAction += moveEvents.ExecuteOnExit;

        _combatState = new State();
        _combatState.EnterAction += combatEvents.ExecuteOnEnter;
        _combatState.UpdateAction += combatEvents.ExecuteOnUpdate;
        _combatState.ExitAction += combatEvents.ExecuteOnExit;

        Transition idleToMoveTransition = new Transition(_idleState, _weakenedState);
        _idleState.AddTransition(idleToMoveTransition);

        Transition moveToDashTransition = new Transition(_weakenedState, _combatState);
        _weakenedState.AddTransition(moveToDashTransition);

        Transition dashToMoveTransition = new Transition(_combatState, _weakenedState);
        _combatState.AddTransition(dashToMoveTransition);

        Transition dashToIdleTransition = new Transition(_combatState, _idleState);
        _combatState.AddTransition(dashToIdleTransition);

        return new List<State>()
        {
            _idleState,
            _weakenedState,
            _combatState
        };
    }
}