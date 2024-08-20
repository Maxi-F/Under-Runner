using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.Input;
using _Dev.UnderRunnerTest.Scripts.ParryProjectile;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Attack Configuration")] [SerializeField]
        private int attackDamage;

        private float attackRadius;
        [SerializeField] private float attackDuration;
        [SerializeField] private float attackCoolDown;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private LayerMask layers;

        private bool _canAttack = true;
        private Coroutine _attackCoroutine = null;
        private bool _isAttacking;

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

            if (_isAttacking)
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
            _isAttacking = true;
            float timer = 0;
            float startTime = Time.time;
            bool hasAttackFinished = false;
            //Turn On Trigger

            while (timer < attackDuration && !hasAttackFinished)
            {
                timer = Time.time - startTime;
                RaycastHit[] hits = Physics.SphereCastAll(attackPoint.position, attackRadius, attackPoint.forward, 0, layers);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.CompareTag("Deflectable"))
                    {
                        if (hit.transform.TryGetComponent<IDeflectable>(out IDeflectable deflectableInterface))
                        {
                            deflectableInterface.Deflect();
                        }

                        hasAttackFinished = true;
                        break;
                    }

                    if (hit.transform.CompareTag("Enemy"))
                    {
                        if (hit.transform.TryGetComponent<ITakeDamage>(out ITakeDamage takeDamageInterface))
                        {
                            takeDamageInterface.TakeDamage(attackDamage);
                        }

                        hasAttackFinished = true;
                        break;
                    }
                }

                yield return null;
            }

            //Turn Off Trigger
            _isAttacking = false;

            yield return CoolDownCoroutine();
            _canAttack = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(attackCoolDown);
        }
    }
}