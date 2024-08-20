using System;
using System.Collections;
using _Dev.GolfTest.Scripts.Events;
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
            Debug.Log($"Handling shield: {isActive}");
            
            shieldObject.SetActive(isActive);
            _healthPoints.SetCanTakeDamage(!isActive);
            onEnemyParriedEvent?.RaiseEvent(isActive);
            
            StartCoroutine(Fly(isActive));
        }

        private IEnumerator Fly(bool isActive)
        {
            _shouldFly = true;

            yield return new WaitForSeconds(flyTime);

            _shouldFly = false;

            if(!isActive)
                StartCoroutine(ReactivateShield());
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
    }
}
