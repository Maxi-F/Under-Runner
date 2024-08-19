using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private IEnemyAttack[] _attacks;
        [SerializeField] private float timeBetweenAttacks;
        
        private IEnemyAttack _actualAttack;
        private bool _shouldExecuteAttack = true;
        
        void Start()
        {
            _attacks ??= GetComponents<IEnemyAttack>();
            SelectRandomAttack();
        }

        private void Update()
        {
            if (_shouldExecuteAttack)
            {
                StartCoroutine(ExecuteAttack());
                _shouldExecuteAttack = false;
            }
        }

        private void SelectRandomAttack()
        {
            _actualAttack = _attacks[Random.Range(0, _attacks.Length - 1)];
        }

        IEnumerator ExecuteAttack()
        {
            _actualAttack.Execute();

            yield return new WaitUntil(() => !_actualAttack.IsExecuting());
            
            yield return new WaitForSeconds(timeBetweenAttacks);

            SelectRandomAttack();
            _shouldExecuteAttack = true;
        }
    }
}
