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

        public override void Enter()
        {
            base.Enter();
            CallCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;

            Debug.Log("Start preparation");
            while (timer < preparationDuration)
            {
                timer = Time.time - startTime;
                _dir = target.transform.position - agentTransform.position;
                _dir.y = 0;
                yield return null;
            }

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