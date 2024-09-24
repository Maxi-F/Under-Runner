using System.Collections;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionMoveController : MinionController
    {
        [SerializeField] private MinionSO minionConfig;


        private Vector3 _moveDir;
        
        public void Enter()
        {
            StartCoroutine(HandleChangeState());
        }

        private IEnumerator HandleChangeState()
        {
            yield return new WaitForSeconds(minionConfig.GetRandomMoveTime());
            MinionAgent.ChangeStateToChargeAttack();
        }

        public void OnUpdate()
        {
            Debug.Log(target);
            if (Vector3.Distance(transform.position, target.transform.position) < minionConfig.moveData.minDistance)
                _moveDir = transform.position - target.transform.position;
            else
                _moveDir = target.transform.position - transform.position;
            
            _moveDir.y = 0;
            transform.Translate(_moveDir * (minionConfig.moveData.speed * Time.deltaTime));
        }
    }
}