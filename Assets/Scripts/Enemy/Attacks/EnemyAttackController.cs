using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Attacks
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
        
        void OnEnable()
        {
            _attacks ??= GetComponents<IEnemyAttack>();

            _shouldExecuteAttack = true;
            _isEnemyParried = false;
            SelectRandomAttack();
            
            onEnemyParriedEvent?.onTypedEvent.AddListener(HandleEnemyParried);
        }

        private void OnDisable()
        {
            onEnemyParriedEvent?.onTypedEvent.RemoveListener(HandleEnemyParried);
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
            yield return _actualAttack.Execute();
            
            onAttackExecutedEvent?.RaiseEvent();
            
            yield return new WaitForSeconds(timeBetweenAttacks);

            SelectRandomAttack();
            _shouldExecuteAttack = true;
        }
    }
}
