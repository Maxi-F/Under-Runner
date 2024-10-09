using System;
using Health;
using UnityEngine;

namespace Minion.Controllers
{
    [RequireComponent(typeof(MinionAgent))]
    public abstract class MinionController : MonoBehaviour
    {
        [HideInInspector] public GameObject target;
        protected MinionAgent MinionAgent;

        protected HealthPoints _healthPoints;
        protected Collider _collider;
        
        public void LookAtTarget()
        {
            Vector3 rotation = Quaternion.LookRotation(target.transform.position).eulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;
            
            MinionAgent.GetModel().transform.rotation = Quaternion.Euler(rotation);
        }
        
        protected virtual void OnEnable()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _collider ??= GetComponent<Collider>();
            MinionAgent ??= GetComponent<MinionAgent>();
            
            target = MinionAgent.GetPlayer();
        }
    }
}