using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Health
{
    public class HealthPoints : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int initHealth = 100;
        [SerializeField] private bool canTakeDamage = true;
        
        [Header("events")] [SerializeField] private VoidEventChannelSO onDeathEvent;
        [SerializeField] private IntEventChannelSO onTakeDamageEvent;

        private bool _isInvincible = false;
        
        public int CurrentHp { get; private set; }

        void Start()
        {
            CurrentHp = initHealth;
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
    }
}
