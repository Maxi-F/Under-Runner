using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using Minion.Manager;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionIdleController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;
        [SerializeField] private MinionAgentEventChannelSO onMinionWantsToAttack;
        private Coroutine _idleTime;
        private bool _canAttack;

        protected override void OnEnable()
        {
            SetIdleCoroutineAsNull();
            base.OnEnable();
        }

        private void OnDisable()
        {
            SetIdleCoroutineAsNull();
        }

        public void Enter()
        {
            transform.rotation = Quaternion.identity;
            if (_idleTime == null)
            {
                _idleTime = StartCoroutine(HandleIdleTime());
            }
        }

        public void SetCanAttack(bool value)
        {
            _canAttack = value;
        }

        private IEnumerator HandleIdleTime()
        {
            _canAttack = false;
            yield return new WaitForSeconds(minionConfig.GetRandomIdleTime());
            onMinionWantsToAttack?.RaiseEvent(minionAgent);
            yield return new WaitUntil(() => _canAttack);
            minionAgent.ChangeStateToMove();
        }

        public void Exit()
        {
            SetIdleCoroutineAsNull();
        }

        private void SetIdleCoroutineAsNull()
        {
            _idleTime = null;
        }
    }
}