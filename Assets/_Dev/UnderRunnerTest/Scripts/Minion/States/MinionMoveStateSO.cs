using _Dev.UnderRunnerTest.Scripts.FSM;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Minion.States
{
    [CreateAssetMenu(fileName = "MinionMoveState", menuName = "Minion/MoveState", order = 0)]
    public class MinionMoveStateSO : StateSO
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("MOVE Enter");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("MOVE Update");
        }
        
        public override void Exit()
        {
            base.Exit();
            Debug.Log("MOVE Exit");
        }
    }
}