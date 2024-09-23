using System.Collections;
using UnityEngine;

namespace Minion.Controllers
{ 
    public class MinionIdleController : MinionController
    {//asd
        [SerializeField] private float timeInIdle;
        public void Enter()
        {
            transform.rotation = Quaternion.identity;
            StartCoroutine(HandleIdleTime());
        }

        private IEnumerator HandleIdleTime()
        {
            yield return new WaitForSeconds(timeInIdle);
            MinionAgent.ChangeStateToMove();
        }
    }
}