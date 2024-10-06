using System.Collections;
using Events;
using Health;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionAttackController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;
        
        private bool _isAttacking;
        public void Enter()
        {
            StartCoroutine(StartCharge());

            _isAttacking = true;
        }
        
        private IEnumerator StartCharge()
        {
            float timer = 0;
            float chargeDuration = minionConfig.chargeAttackData.length / minionConfig.attackData.speed;
            float startTime = Time.time;
            Vector3 dir = target.transform.position - transform.position;
            
            Vector3 destination = transform.position + dir.normalized * minionConfig.chargeAttackData.length;
            Vector3 startPosition = transform.position;
            destination.y = startPosition.y;
            
            _healthPoints.SetCanTakeDamage(false);
            _collider.isTrigger = true;
            
            while (timer < chargeDuration)
            {
                timer = Time.time - startTime;
                transform.position = Vector3.Lerp(startPosition, destination, timer / chargeDuration);
                yield return null;
            }
            _healthPoints.SetCanTakeDamage(true);

            MinionAgent.ChangeStateToFallingBack();
        }

        public void Exit()
        {
            Debug.Log($"{gameObject.GetInstanceID()}: Exiting attack state");
            _isAttacking = false;
            MinionAgent.SetIsNotInAttackState();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && _isAttacking)
            {
                DealDamage(other.gameObject);
            }
        }
        
        private void DealDamage(GameObject target)
        {
            target.gameObject.TryGetComponent(out ITakeDamage playerHealth);
            playerHealth.TryTakeDamage(minionConfig.attackData.damage);
        }
    }
}
