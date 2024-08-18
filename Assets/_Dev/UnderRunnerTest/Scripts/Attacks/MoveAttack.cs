using System;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks
{
    public class MoveAttack : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float velocity;
        [SerializeField] private float distanceToInactivate = 0.1f;
        
        // Update is called once per frame
        void Update()
        {
            Vector3 direction = (endPosition - transform.position).normalized;
            transform.position += direction * (velocity * Time.deltaTime);

            if ((transform.position - endPosition).magnitude <= distanceToInactivate)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Collided with player!");
            }
        }

        public void ResetAttack()
        {
            transform.position = startPosition;
            
            gameObject.SetActive(true);
        }
    }
}
