using System;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Health
{
    public class HealthPoints : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int initHealth = 100;
        [SerializeField] private bool canTakeDamage = true;

        [Header("events")] 
        [SerializeField] private VoidEventChannelSO onDeathEvent;
        [SerializeField] private IntEventChannelSO onTakeDamageEvent;
        [SerializeField] private IntEventChannelSO onResetPointsEvent;
        
        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public VoidEventChannelSO OnDeathEvent
        {
            get { return onDeathEvent; }
        }

        private bool _isInvincible = false;

        public int CurrentHp { get; private set; }

        void Start()
        {
            CurrentHp = initHealth;
        }

        private void OnDestroy()
        {
            if (onDeathEvent != null)
                onDeathEvent.onEvent?.RemoveAllListeners();
        }

        public void SetCanTakeDamage(bool value)
        {
            canTakeDamage = value;
        }

        // Is invincible =/= can take damage.
        // isInvincible is used for attacks That can be avoidable.
        // canTakeDamage is used if the entity just cant take damage in any way.
        public void SetIsInvincible(bool value)
        {
            _isInvincible = value;
        }

        public void ResetHitPoints()
        {
            CurrentHp = maxHealth;
            onResetPointsEvent?.RaiseEvent(CurrentHp);
        }

        public void TakeDamage(int damage)
        {
            if (!canTakeDamage)
            {
                return;
            }

            CurrentHp -= damage;

            if (IsDead())
            {
                onDeathEvent?.RaiseEvent();
            }
            else
            {
                onTakeDamageEvent?.RaiseEvent(CurrentHp);
            }
        }

        public bool IsDead()
        {
            return CurrentHp <= 0;
        }

        public void TryTakeAvoidableDamage(int damage)
        {
            if (_isInvincible) return;
            TakeDamage(damage);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public void ToggleInvulnerability()
        {
            canTakeDamage = !canTakeDamage;
        }

        public void ToggleInvulnerability(bool value)
        {
            canTakeDamage = value;
        }
#endif
    }
}