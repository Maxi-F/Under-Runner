using System.Collections;
using Events;
using Health;
using UnityEngine;

namespace Minion.Controllers
{ public class MinionChargeAttackController : MinionController
    {
        [SerializeField] private float preparationDuration;
        [SerializeField] private int attackDamage;
        [SerializeField] private float ChargeLength;

        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;
        
        private LineRenderer _aimLine;
        private Vector3 _dir;
        private bool _isCharging;
        
        public void Enter()
        {
            _aimLine ??= gameObject.transform.Find("AimLine").gameObject.GetComponent<LineRenderer>();

            
            onCollidePlayerEventChannel.onGameObjectEvent.AddListener(DealDamage);

            StartCoroutine(AttackCoroutine());
        }

        private void StartAimLine()
        {
            _aimLine.SetPosition(0, transform.position);
            _aimLine.SetPosition(1, transform.position);
            _aimLine.enabled = true;
        }

        private void SetNewAimPosition(float timer)
        {
            _dir = target.transform.position - transform.position;
            _dir.y = 0;

            Vector3 aimPosition = Vector3.Lerp(transform.position, transform.position + _dir.normalized * ChargeLength, timer / preparationDuration);
            _aimLine.SetPosition(1, aimPosition);
        }
        
        private IEnumerator AttackCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            StartAimLine();

            Debug.Log("Start preparation");
            while (timer < preparationDuration)
            {
                timer = Time.time - startTime;
                SetNewAimPosition(timer);
                yield return null;
            }

            _aimLine.enabled = false;
            Debug.Log("Start Charge");
            
            MinionAgent.ChangeStateToAttack();
        }

        private void DealDamage(GameObject target)
        {
            target.gameObject.TryGetComponent(out ITakeDamage playerHealth);
            playerHealth.TakeDamage(attackDamage);
        }
    }
}