using _Dev.GolfTest.Scripts.Events;
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
        
        public int CurrentHp { get; private set; }

        void Start()
        {
            CurrentHp = initHealth;
        }

        public void SetCanTakeDamage(bool value)
        {
            canTakeDamage = value;
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

            if (CurrentHp <= 0)
            {
                onDeathEvent?.RaiseEvent();
            }
            else
            {
                onTakeDamageEvent?.RaiseEvent(CurrentHp);
            }
        }
    }
}
