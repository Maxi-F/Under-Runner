using System.Collections;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionAttackController : MinionController
    {
        [SerializeField] private float ChargeLength;
        [SerializeField] private float ChargeSpeed;
        
        public void Enter()
        {
            StartCoroutine(StartCharge());
        }
        
        private IEnumerator StartCharge()
        {
            float timer = 0;
            float chargeDuration = ChargeLength / ChargeSpeed;
            float startTime = Time.time;
            Vector3 dir = target.transform.position - transform.position;
            
            Vector3 destination = transform.position + dir.normalized * ChargeLength;
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
            
            MinionAgent.ChangeStateToIdle();
        }
    }
}
