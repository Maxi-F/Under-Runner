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
        [SerializeField] private BoolEventChannelSO onCanMinionsAttackEvent;
        
        private Coroutine _idleTime;
        private bool _canAttack;

        protected override void OnEnable()
        {
            SetIdleCoroutineAsNull();
            onCanMinionsAttackEvent?.onTypedEvent.AddListener(SetCanAttack);
            base.OnEnable();
        }
        
        private void OnDisable()
        {
            onCanMinionsAttackEvent?.onTypedEvent.RemoveListener(SetCanAttack);
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
            yield return new WaitForSeconds(minionConfig.GetRandomIdleTime());
            yield return new WaitUntil(() => _canAttack);
            MinionAgent.SetIsInAttackState();
            MinionAgent.ChangeStateToMove();
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
