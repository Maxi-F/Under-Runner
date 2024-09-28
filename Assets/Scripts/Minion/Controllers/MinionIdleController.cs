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
        [SerializeField] private VoidEventChannelSO onMinionAttackingEvent;
        public void Enter()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(HandleIdleTime());
        }

        private IEnumerator HandleIdleTime()
        {
            yield return new WaitForSeconds(minionConfig.GetRandomIdleTime());
            yield return new WaitUntil(() => MinionManager.CanAttack);
            onMinionAttackingEvent?.RaiseEvent();
            MinionAgent.SetIsInAttackState();
            MinionAgent.ChangeStateToMove();
        }
    }
}
