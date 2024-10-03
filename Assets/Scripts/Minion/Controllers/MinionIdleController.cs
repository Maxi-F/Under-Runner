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

        public void Enter()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(HandleIdleTime());
        }

        private IEnumerator HandleIdleTime()
        {
            yield return new WaitForSeconds(minionConfig.GetRandomIdleTime());
            yield return new WaitUntil(() => MinionManager.CanAttack);
            MinionAgent.SetIsInAttackState();
            MinionAgent.ChangeStateToMove();
        }
    }
}
