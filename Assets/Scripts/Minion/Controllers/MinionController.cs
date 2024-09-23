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
        
        private void Start()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _collider ??= GetComponent<Collider>();
            MinionAgent ??= GetComponent<MinionAgent>();
            
            Debug.Log($"test {MinionAgent}");
            target = MinionAgent.GetPlayer();
            
            Debug.Log($"target {target}");
        }
    }
}