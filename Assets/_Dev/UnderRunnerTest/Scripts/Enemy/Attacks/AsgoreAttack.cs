using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Attacks;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class AsgoreAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private MoveAttack nonMoveAttack;
        [SerializeField] private MoveAttack moveAttack;

        private bool _isExecuting;
        
        public void Execute()
        {
            _isExecuting = true;

            StartCoroutine(ExecuteMoveAttack());
        }

        public bool IsExecuting()
        {
            return _isExecuting;
        }

        private IEnumerator ExecuteMoveAttack()
        {
            nonMoveAttack.ResetAttack();

            yield return new WaitUntil(() => nonMoveAttack.HasFinished());
            
            moveAttack.ResetAttack();

            yield return new WaitUntil(() => moveAttack.HasFinished());

            _isExecuting = false;
        }
    }
}
