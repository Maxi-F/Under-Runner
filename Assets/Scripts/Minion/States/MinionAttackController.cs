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

        [SerializeField] private Material defaultMat;
        [SerializeField] private Material chargeMat;
        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

        [SerializeField] private MinionAgent minionAgent;
        
        private Vector3 _dir;

        private LineRenderer _aimLine;


        public override void Enter()
        {
            _aimLine = agentTransform.Find("AimLine").gameObject.GetComponent<LineRenderer>();

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
            foreach (MeshRenderer meshRenderer in agentTransform.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = chargeMat;
            }

            agentTransform.GetComponent<Collider>().isTrigger = true;
            while (timer < chargeDuration)
            {
                timer = Time.time - startTime;
                agentTransform.position = Vector3.Lerp(startPosition, destination, timer / chargeDuration);
                yield return null;
            }

            agentTransform.GetComponent<HealthPoints>().SetCanTakeDamage(true);

            foreach (MeshRenderer meshRenderer in agentTransform.GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.material = defaultMat;
            }

            agentTransform.GetComponent<Collider>().isTrigger = false;
            
            minionAgent.ChangeStateToIdle();
        }

        private void DealDamage(GameObject target)
        {
            target.gameObject.TryGetComponent(out ITakeDamage playerHealth);
            playerHealth.TakeDamage(attackDamage);
        }
    }
}