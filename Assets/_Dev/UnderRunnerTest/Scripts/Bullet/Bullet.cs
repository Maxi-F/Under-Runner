using System;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Bullet
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Vector3 direction = Vector3.forward;
        [SerializeField] private float velocity = 10.0f;

        [SerializeField] private int damage = 5;
        
        void Update()
        {
            transform.position += direction * (velocity * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Triggered!");
            if (other.CompareTag("Enemy"))
            {
                ITakeDamage enemy = other.GetComponentInChildren<ITakeDamage>();
                
                enemy?.TakeDamage(damage);
                
                Destroy(gameObject);
            }
        }
    }
}
