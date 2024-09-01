using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private GameObject enemyObject;
        [SerializeField] private GameObject shieldObject;
        [SerializeField] private bool shieldActive;

        [Header("ShieldProperties")] [SerializeField]
        private float timeToReactivateShield = 4.0f;
        [SerializeField] private HealthPoints shieldPoints;
        
        [Header("FlyProperties")] 
        [SerializeField] private float flyVelocity = 10.0f;
        [SerializeField] private float flyTime = 2.0f;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onEnemyDeathEvent;
        [SerializeField] private BoolEventChannelSO onEnemyParriedEvent;
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        
        private HealthPoints _healthPoints;
        private bool _shouldFly;
        
        private void Start()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            
            _healthPoints.SetCanTakeDamage(false);
            
            onEnemyDeathEvent?.onEvent.AddListener(HandleDeath);
        }

        private void Update()
        {
            if(_shouldFly)
                enemyObject.transform.position +=
                    new Vector3(
                        0f, 
                        (shieldObject.activeInHierarchy ? 1f : -1f) * flyVelocity * Time.deltaTime, 
                        0f
                        );
        }

        public void HandleShield(bool isActive)
        {
            if (!isActive && !shieldObject.activeInHierarchy) return;
            
            shieldObject.SetActive(isActive);
            _healthPoints.SetCanTakeDamage(!isActive);

            if (!isActive)
            {
                onEnemyParriedEvent?.RaiseEvent(isActive);
            }
            else
            {
                shieldPoints.ResetHitPoints();
            }
            
            StartCoroutine(Fly(isActive));
        }

        private IEnumerator Fly(bool isActive)
        {
            _shouldFly = true;

            yield return new WaitForSeconds(flyTime);

            _shouldFly = false;

            if (!isActive)
            {
                StartCoroutine(ReactivateShield());
            }
            else
            {
                onEnemyParriedEvent?.RaiseEvent(isActive);
            }
        }

        private IEnumerator ReactivateShield()
        {
            yield return new WaitForSeconds(timeToReactivateShield);
            
            HandleShield(true);
        }

        private void OnDisable()
        {
            onEnemyDamageEvent?.onEvent.RemoveListener(HandleDeath);
        }

        private void HandleDeath()
        {
            enemyObject.SetActive(false);
        }

        public bool TryDestroyShield(int parryDamage)
        {
            shieldPoints.TakeDamage(parryDamage);

            if (shieldPoints.IsDead())
            {
                HandleShield(false);
                return true;
            }

            return false;
        }
    }
}