using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Attack Configuration")] [SerializeField]
        private float attackRadius;

        [SerializeField] private float attackDuration;
        [SerializeField] private float attackCoolDown;
        [SerializeField] private Transform attackPoint;

        private bool _canAttack = true;
        private Coroutine _attackCoroutine = null;
        private bool? _sphereCastResult = null;

        private void OnEnable()
        {
            inputHandler.onPlayerAttack.AddListener(HandleAttack);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerAttack.RemoveListener(HandleAttack);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            if (_sphereCastResult != null)
            {
                Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
            }
        }

        public void HandleAttack()
        {
            if (!_canAttack)
                return;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            float timer = 0;
            float startTime = Time.time;

            //Turn On Trigger
            while (timer < attackDuration)
            {
                timer = Time.time - startTime;
                _sphereCastResult = Physics.SphereCast(attackPoint.position, attackRadius, transform.forward, out RaycastHit hit);
                yield return null;
            }

            //Turn Off Trigger
            _sphereCastResult = null;

            yield return CoolDownCoroutine();
            _canAttack = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(attackCoolDown);
        }
    }
}