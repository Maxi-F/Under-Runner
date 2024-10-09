using System;
using System.Collections;
using Enemy.Shield;
using Events;
using Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Properties")] 
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
        
        private void OnEnable()
        {
            _healthPoints ??= GetComponent<HealthPoints>();
            
            _healthPoints.SetCanTakeDamage(false);
            shieldController.SetActive(true);
            shieldController.SetActiveMaterial();
            
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
            gameObject.SetActive(false);
        }

        public bool TryDestroyShield(int parryDamage)
        {
            shieldPoints.TryTakeDamage(parryDamage);

            if (shieldPoints.IsDead())
            {
                HandleShield(false);
                return true;
            }

            return false;
        }
    }
}
