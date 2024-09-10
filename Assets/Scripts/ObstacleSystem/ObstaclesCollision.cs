using System;
using Enemy.Attacks;
using Health;
using UnityEngine;

namespace ObstacleSystem
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