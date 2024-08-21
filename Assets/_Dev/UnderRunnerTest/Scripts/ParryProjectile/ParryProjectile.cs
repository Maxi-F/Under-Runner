using System;
using _Dev.UnderRunnerTest.Scripts.Enemy;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.ParryProjectile
{
    public class ParryProjectile : MonoBehaviour, IDeflectable
    {
        [SerializeField] private float velocity = 5.0f;
        [SerializeField] private int damage = 5;
        
        private GameObject _objectToFollow;
        
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _objectToFollow.transform.position,
                velocity * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Parry projectile collided!");

            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponentInChildren<EnemyController>();

                enemy.HandleShield(false);
                gameObject.SetActive(false);
            } else if (other.CompareTag("Player"))
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                
                damageTaker.TakeDamage(damage);
            }
        }

        public void SetObjectToFollow(GameObject newObjectToFollow)
        {
            _objectToFollow = newObjectToFollow;
        }

        public void Deflect(GameObject newObjectToFollow)
        {
            Debug.Log("Parry");
            SetObjectToFollow(newObjectToFollow);
        }
    }
}