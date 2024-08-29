using System;
using System.Collections;
using System.Linq;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class EnemyAttackController : MonoBehaviour
    {
        [SerializeField] private float timeBetweenAttacks;

        [Header("events")] 
        [SerializeField] private BoolEventChannelSO onEnemyParriedEvent;

        [SerializeField] private VoidEventChannelSO onAttackExecutedEvent;
        
        private IEnemyAttack _actualAttack;
        private bool _shouldExecuteAttack = true;
        private bool _isEnemyParried = false;
        private IEnemyAttack[] _attacks;
        
        void Start()
        {
            _attacks ??= GetComponents<IEnemyAttack>();
            
            SelectRandomAttack();
            
            onEnemyParriedEvent?.onBoolEvent.AddListener(HandleEnemyParried);
        }

        private void OnDisable()
        {
            onEnemyParriedEvent?.onBoolEvent.RemoveListener(HandleEnemyParried);
        }

        private void HandleEnemyParried(bool isShieldActive)
        {
            _isEnemyParried = !isShieldActive;
        }

        private void Update()
        {
            if (_shouldExecuteAttack && !_isEnemyParried)
            {
                StartCoroutine(ExecuteAttack());
                _shouldExecuteAttack = false;
            }
        }

        private void SelectRandomAttack()
        {
            IEnemyAttack[] attacksToSearchFrom = _attacks.Where((attack) => attack.CanExecute()).ToArray();
            _actualAttack = attacksToSearchFrom[Random.Range(0, attacksToSearchFrom.Length)];
        }

        IEnumerator ExecuteAttack()
        {
            _actualAttack.Execute();
            onAttackExecutedEvent?.RaiseEvent();
            
            yield return new WaitUntil(() => !_actualAttack.IsExecuting());
            
            yield return new WaitForSeconds(timeBetweenAttacks);

            SelectRandomAttack();
            _shouldExecuteAttack = true;
        }
    }
}
