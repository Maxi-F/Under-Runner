using System;
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
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onEnemyDeathEvent;
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        
        private HealthPoints _healthPoints;
        private void Start()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            
            _healthPoints.SetCanTakeDamage(false);
            
            onEnemyDeathEvent?.onEvent.AddListener(HandleDeath);
        }

        private void HandleShield(bool isActive)
        {
            shieldObject.SetActive(isActive);
            _healthPoints.SetCanTakeDamage(!isActive);
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
