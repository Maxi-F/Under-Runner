using System.Collections;
using System.Collections.Generic;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Minion.States
{
    [CreateAssetMenu(fileName = "MinionAttackState", menuName = "Minion/AttackState", order = 0)]
    public class MinionAttackStateSO : MinionStateSO
    {
        [SerializeField] private float preparationDuration;
        [SerializeField] private int attackDamage;
        [SerializeField] private float ChargeLength;
        [SerializeField] private float ChargeSpeed;

        [SerializeField] private Material defaultMat;
        [SerializeField] private Material chargeMat;
        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

        private Vector3 _dir;

        private LineRenderer _aimLine;


        public override void Enter()
        {
            base.Enter();
            _aimLine = agentTransform.Find("AimLine").gameObject.GetComponent<LineRenderer>();
            CallCoroutine(AttackCoroutine());

            onCollidePlayerEventChannel.onGameObjectEvent.AddListener(DealDamage);
        }

        public override void Exit()
        {
            base.Exit();
            onCollidePlayerEventChannel.onGameObjectEvent.RemoveListener(DealDamage);
        }

        private IEnumerator AttackCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            _aimLine.SetPosition(0, agentTransform.position);
            _aimLine.SetPosition(1, agentTransform.position);
            _aimLine.enabled = true;

            Debug.Log("Start preparation");
            while (timer < preparationDuration)
            {
                timer = Time.time - startTime;
                _dir = target.transform.position - agentTransform.position;
                _dir.y = 0;

                Vector3 aimPosition = Vector3.Lerp(agentTransform.position, agentTransform.position + _dir.normalized * ChargeLength, timer / preparationDuration);
                _aimLine.SetPosition(1, aimPosition);
                yield return null;
            }

            _aimLine.enabled = false;
            Debug.Log("Start Charge");

            Vector3 destination = agentTransform.position + _dir.normalized * ChargeLength;
            Vector3 startPosition = agentTransform.position;
            timer = 0;
            float chargeDuration = ChargeLength / ChargeSpeed;
            startTime = Time.time;
            agentTransform.GetComponent<HealthPoints>().SetCanTakeDamage(false);
            agentTransform.GetComponent<MeshRenderer>().material = chargeMat;
            agentTransform.GetComponent<Collider>().isTrigger = true;
            while (timer < chargeDuration)
            {
                timer = Time.time - startTime;
                agentTransform.position = Vector3.Lerp(startPosition, destination, timer / chargeDuration);
                yield return null;
            }

            agentTransform.GetComponent<HealthPoints>().SetCanTakeDamage(true);
            agentTransform.GetComponent<MeshRenderer>().material = defaultMat;
            agentTransform.GetComponent<Collider>().isTrigger = false;
        }

        private void DealDamage(GameObject target)
        {
            target.gameObject.TryGetComponent(out ITakeDamage playerHealth);
            playerHealth.TakeDamage(attackDamage);
        }
    }
}