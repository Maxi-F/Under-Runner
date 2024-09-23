using System.Collections;
using UnityEngine;

namespace Minion.Controllers
{
    public class MinionMoveController : MinionController
    {
        [SerializeField] private float timeMoving;
        [SerializeField] private float speed;
        [SerializeField] private float minDistance;


        private Vector3 _moveDir;
        
        public void Enter()
        {
            StartCoroutine(HandleChangeState());
        }

        private IEnumerator HandleChangeState()
        {
            yield return new WaitForSeconds(timeMoving);
            MinionAgent.ChangeStateToChargeAttack();
        }

        public void OnUpdate()
        {
            Debug.Log(target);
            if (Vector3.Distance(transform.position, target.transform.position) < minDistance)
                _moveDir = transform.position - target.transform.position;
            else
                _moveDir = target.transform.position - transform.position;
            
            _moveDir.y = 0;
            transform.Translate(_moveDir * (speed * Time.deltaTime));
        }
    }
}