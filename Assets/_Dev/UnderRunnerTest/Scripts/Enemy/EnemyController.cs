using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Enemy.Shield;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Dev.UnderRunnerTest.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private GameObject enemyObject;
        [SerializeField] private ShieldController shieldController;
        [SerializeField] private bool shieldActive;

        [Header("ShieldProperties")] 
        [SerializeField] private float timeToReactivateShield = 4.0f;

        [SerializeField] private float timeToStartReactivatingShield = 2.0f;
        
        [SerializeField] private HealthPoints shieldPoints;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onEnemyDeathEvent;
        [SerializeField] private BoolEventChannelSO onEnemyParriedEvent;
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        
        private HealthPoints _healthPoints;
        
        private void Start()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            
            _healthPoints.SetCanTakeDamage(false);
            
            onEnemyDeathEvent?.onEvent.AddListener(HandleDeath);
        }

        public void HandleShield(bool isActive)
        {
            if (!isActive && !shieldController.IsActive()) return;
            
            shieldController.SetActive(isActive);
            _healthPoints.SetCanTakeDamage(!isActive);

            onEnemyParriedEvent?.RaiseEvent(isActive);
           
            if (isActive)
            {
                shieldPoints.ResetHitPoints();
            }
            else
            {
                StartCoroutine(ReactivateShield());
            }
        }

        private IEnumerator ReactivateShield()
        {
            yield return new WaitForSeconds(timeToStartReactivatingShield);
            
            shieldController.SetIsActivating(true);
            
            yield return new WaitForSeconds(timeToReactivateShield);
            
            shieldController.SetActiveMaterial();
            
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
