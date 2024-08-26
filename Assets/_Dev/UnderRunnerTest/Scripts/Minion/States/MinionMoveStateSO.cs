using System.Diagnostics;
using _Dev.UnderRunnerTest.Scripts.FSM;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Dev.UnderRunnerTest.Scripts.Minion.States
{
    [CreateAssetMenu(fileName = "MinionMoveState", menuName = "Minion/MoveState", order = 0)]
    public class MinionMoveStateSO : MinionStateSO
    {
        [SerializeField] private float speed;
        [SerializeField] private float minDistance;

        private Vector3 _dir;

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            
            if (Vector3.Distance(agentTransform.position, target.transform.position) < minDistance)
                _dir = agentTransform.position - target.transform.position;
            else
                _dir = target.transform.position - agentTransform.position;

            _dir.y = 0;
            agentTransform.Translate(_dir * (speed * Time.deltaTime));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}