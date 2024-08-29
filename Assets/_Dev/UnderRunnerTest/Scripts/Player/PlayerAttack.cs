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

        [SerializeField] private GameObject attackSphere;
        [SerializeField] private float attackRadius;
        [SerializeField] private float attackDuration;
        [SerializeField] private float attackCoolDown;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private LayerMask layers;

        [Header("Enemy")] [SerializeField] private GameObject enemy;
        
        private bool _canAttack = true;
        private Coroutine _attackCoroutine = null;
        
        private void OnEnable()
        {
            inputHandler.onPlayerAttack.AddListener(HandleAttack);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerAttack.RemoveListener(HandleAttack);
        }

        public void HandleAttack()
        {
            if (!_canAttack)
                return;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
        
        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            attackSphere.SetActive(true);
            
            float timer = 0;
            float startTime = Time.time;
            bool hasAttackFinished = false;

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
                            deflectableInterface.Deflect(enemy);
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

            attackSphere.SetActive(false);

            yield return CoolDownCoroutine();
            _canAttack = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(attackCoolDown);
        }
    }
}