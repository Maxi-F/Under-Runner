using System;
using Health;
using UnityEngine;

namespace Minion.Controllers
{
    public abstract class MinionController : MonoBehaviour
    {
        [HideInInspector] public GameObject target;

        protected HealthPoints _healthPoints;
        protected Collider _collider;
        
        private void Start()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            _collider ??= GetComponent<Collider>();
        }

        public abstract void Enter();
        public abstract void OnUpdate();
        public abstract void Exit();
    }
}