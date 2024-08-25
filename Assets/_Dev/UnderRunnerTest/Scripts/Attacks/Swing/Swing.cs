using System;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [SerializeField] private int damage = 5;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                
                damageTaker.TakeDamage(damage);
            }
        }
    }
}
