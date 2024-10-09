using System;
using System.Collections;
using Events;
using Minion.Manager;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{ 
    public class MinionIdleController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;

        private Coroutine _idleTime;

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
            else
            {
                Debug.LogError("PLEASEE");
            }
        }

        private IEnumerator HandleIdleTime()
        {
            yield return new WaitForSeconds(minionConfig.GetRandomIdleTime());
            yield return new WaitUntil(() => MinionManager.CanAttack);
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
