using System.Collections;
using UnityEngine;

namespace Minion.Controllers
{ 
    public class MinionIdleController : MinionController
    {
        [SerializeField] private float timeInIdle;
        [SerializeField] private MinionAgent minionAgent;
        
        public override void Enter()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(HandleIdleTime());
        }

        private IEnumerator HandleIdleTime()
        {
            yield return new WaitForSeconds(timeInIdle);
            minionAgent.ChangeStateToMove();
        }

        public override void OnUpdate()
        {
        }

        public override void Exit()
        {
        }
    }
}