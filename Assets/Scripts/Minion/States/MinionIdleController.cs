using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minion.States
{ 
    public class MinionIdleController : MinionController
    {
        [SerializeField] private float timeInIdle;
        [SerializeField] private MinionAgent minionAgent;
        
        public override void Enter()
        {
            Debug.Log("Enter minion idle");
            StartCoroutine(HandleIdleTime());
        }

        private IEnumerator HandleIdleTime()
        {
            Debug.Log(Time.timeScale);
            yield return null;
            Debug.Log("maybe?");
            Debug.Log(timeInIdle);
            yield return new WaitForSeconds(timeInIdle);
            Debug.Log("HERE!");
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