using _Dev.UnderRunnerTest.Scripts.FSM;
using UnityEditor.Experimental;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Minion.States
{
    [CreateAssetMenu(fileName = "MinionIdleState", menuName = "Minion/IdleState", order = 0)]
    public class MinionIdleStateSO : StateSO
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("Idle Update");
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}