using System;
using _Dev.GolfTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks
{
    public class MoveAttack : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float distanceToInactivate = 0.1f;
        [SerializeField] private bool isInMoveAttack = true;

        [Header("Events")] [SerializeField] private BoolEventChannelSO onIsPlayerMoving;

        [Header("Attack Velocity")] [SerializeField]
        private float velocity = 10.0f;
        [SerializeField] private AnimationCurve velocityInTime;
        
        private bool _isPlayerMoving;
        private float _velocityTime = 0f;
        
        void Start()
        {
            onIsPlayerMoving.onBoolEvent.AddListener(HandleIsMoving);
        }

        void OnDisable()
        {
            onIsPlayerMoving.onBoolEvent.RemoveListener(HandleIsMoving);
        }

        // Update is called once per frame
        void Update()
        {
            float velocityToUse = velocity * velocityInTime.Evaluate(_velocityTime);
            
            Vector3 direction = (endPosition - transform.position).normalized;
            transform.position += direction * (velocityToUse * Time.deltaTime);

            _velocityTime += Time.deltaTime;
            if ((transform.position - endPosition).magnitude <= distanceToInactivate)
            {
                gameObject.SetActive(false);
                _velocityTime = 0f;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            /* TODO Add attack player logic */
            if (other.CompareTag("Player"))
            {
                Debug.Log("Collided with player!");

                if (isInMoveAttack && _isPlayerMoving)
                {
                    Debug.Log("Player was moving while move attack occured.");
                } else if (!isInMoveAttack && !_isPlayerMoving)
                {
                    Debug.Log("Player was not moving while on not move attack occured.");
                }
            }
        }

        private void HandleIsMoving(bool isMoving)
        {
            _isPlayerMoving = isMoving;
        }

        public void ResetAttack()
        {
            transform.position = startPosition;
            
            gameObject.SetActive(true);
        }
    }
}
