using System.Collections;
using System.Collections.Generic;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minion.States
{ public class MinionAttackController : MinionController
    {
        [SerializeField] private float preparationDuration;
        [SerializeField] private int attackDamage;
        [SerializeField] private float ChargeLength;
        [SerializeField] private float ChargeSpeed;

        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

        [SerializeField] private MinionAgent minionAgent;
        
        private Vector3 _dir;

        private LineRenderer _aimLine;
        private HealthPoints _healthPoints;
        private Collider _collider;

        private bool _isCharging;
        
        public override void Enter()
        {
            _aimLine ??= gameObject.transform.Find("AimLine").gameObject.GetComponent<LineRenderer>();
            _healthPoints ??= GetComponent<HealthPoints>();
            _collider ??= GetComponent<Collider>();
            
            onCollidePlayerEventChannel.onGameObjectEvent.AddListener(DealDamage);

            StartCoroutine(AttackCoroutine());
        }

        public override void OnUpdate()
        {
        }

        public override void Exit()
        {
            onCollidePlayerEventChannel.onGameObjectEvent.RemoveListener(DealDamage);
        }

        private void StartAimLine()
        {
            _aimLine.SetPosition(0, transform.position);
            _aimLine.SetPosition(1, transform.position);
            _aimLine.enabled = true;
        }

        private void SetNewAimPosition(float timer)
        {
            _dir = target.transform.position - transform.position;
            _dir.y = 0;

            Vector3 aimPosition = Vector3.Lerp(transform.position, transform.position + _dir.normalized * ChargeLength, timer / preparationDuration);
            _aimLine.SetPosition(1, aimPosition);
        }
        
        private IEnumerator AttackCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            StartAimLine();

            Debug.Log("Start preparation");
            while (timer < preparationDuration)
            {
                timer = Time.time - startTime;
                SetNewAimPosition(timer);
                yield return null;
            }

            _aimLine.enabled = false;
            Debug.Log("Start Charge");

            _isCharging = true;
            StartCoroutine(StartCharge());
            yield return new WaitUntil(() => !_isCharging);

            _healthPoints.SetCanTakeDamage(true);
            _collider.isTrigger = false;
            
            minionAgent.ChangeStateToIdle();
        }

        private IEnumerator StartCharge()
        {
            float timer = 0;
            float chargeDuration = ChargeLength / ChargeSpeed;
            float startTime = Time.time;
            
            Vector3 destination = transform.position + _dir.normalized * ChargeLength;
            Vector3 startPosition = transform.position;
            
            _healthPoints.SetCanTakeDamage(false);
            _collider.isTrigger = true;
            
            while (timer < chargeDuration)
            {
                timer = Time.time - startTime;
                transform.position = Vector3.Lerp(startPosition, destination, timer / chargeDuration);
                yield return null;
            }

            _isCharging = false;
        }

        private void DealDamage(GameObject target)
        {
            target.gameObject.TryGetComponent(out ITakeDamage playerHealth);
            playerHealth.TakeDamage(attackDamage);
        }
    }
}