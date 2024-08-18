using System;
using _Dev.UnderRunnerTest.Scripts.Enemy;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.ParryProjectile
{
    public class ParryProjectile : MonoBehaviour
    {
        [SerializeField] private GameObject objectToFollow;
        [SerializeField] private float velocity = 5.0f;
        
        private void Update()
        {
            Vector3 direction = (objectToFollow.transform.position - transform.position).normalized;
            transform.position +=  direction * (velocity * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Parry projectile collided!");

            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponentInChildren<EnemyController>();
                
                enemy.HandleShield(false);
                gameObject.SetActive(false);
            }
        }
    }
}
