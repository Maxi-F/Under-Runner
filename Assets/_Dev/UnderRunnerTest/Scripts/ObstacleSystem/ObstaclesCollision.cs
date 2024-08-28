using System;
using _Dev.UnderRunnerTest.Scripts.Enemy.Attacks;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.ObstacleSystem
{
    public class ObstaclesCollision : MonoBehaviour
    {
        [SerializeField] private int collisionDamage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.TryGetComponent<ITakeDamage>(out ITakeDamage playerHealth);
                playerHealth.TakeDamage(collisionDamage);
                Destroy(gameObject);
            }
        }
    }
}