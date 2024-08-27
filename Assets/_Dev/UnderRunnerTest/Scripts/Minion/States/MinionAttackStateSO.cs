using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Dev.UnderRunnerTest.Scripts.Minion.States
{
    [CreateAssetMenu(fileName = "MinionAttackState", menuName = "Minion/AttackState", order = 0)]
    public class MinionAttackStateSO : MinionStateSO
    {
        [SerializeField] private float preparationDuration;
        [SerializeField] private float ChargeLength;
        [SerializeField] private float ChargeSpeed;

        private Vector3 _dir;

        private LineRenderer _aimLine;

        public override void Enter()
        {
            base.Enter();
            _aimLine = agentTransform.Find("AimLine").gameObject.GetComponent<LineRenderer>();
            CallCoroutine(AttackCoroutine());
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

            while (timer < chargeDuration)
            {
                timer = Time.time - startTime;
                agentTransform.position = Vector3.Lerp(startPosition, destination, timer / chargeDuration);
                yield return null;
            }
        }
    }
}