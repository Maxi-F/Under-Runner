using System;
using _Dev.UnderRunnerTest.Scripts.FSM;
using _Dev.UnderRunnerTest.Scripts.Minion.States;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Dev.UnderRunnerTest.Scripts.Minion
{
    public class MinionAgent : Agent
    {
        [ContextMenu("Move")]
        private void ChangeToMoveState()
        {
            foreach (StateSO state in config.states)
            {
                if (state as MinionMoveStateSO != null)
                {
                    fsm.ChangeState(state);
                }
            }
        }
        
        [ContextMenu("Idle")]
        private void ChangeToIdleState()
        {
            foreach (StateSO state in config.states)
            {
                if (state as MinionIdleStateSO != null)
                {
                    fsm.ChangeState(state);
                }
            }
        }
    }
}