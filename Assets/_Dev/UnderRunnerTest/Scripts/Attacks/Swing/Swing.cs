using System;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [SerializeField] private int damage = 5;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onSwingEndEvent;

        private void OnEnable()
        {
            onSwingEndEvent?.onEvent.AddListener(HandleSwingEndEvent);
        }

        private void OnDisable()
        {
            onSwingEndEvent?.onEvent.RemoveListener(HandleSwingEndEvent);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                
                damageTaker.TryTakeAvoidableDamage(damage);
            }
        }

        private void HandleSwingEndEvent()
        {
            gameObject.SetActive(false);
        }
    }
}
