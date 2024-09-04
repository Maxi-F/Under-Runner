using System;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy
{
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField] private Vector2 minMaxYMovement;
        [SerializeField] private float distanceToChangeDirection = 0.2f;
        
        [SerializeField] private GameObject enemyObject;
        
        private Vector3 _originPosition;

        private bool _shouldGoDown = false;
        
        void OnEnable()
        {
            _originPosition = transform.position;
        }
        
        void Update()
        {
            Vector3 newPosition = GetHoverPosition();

            enemyObject.transform.position = newPosition;
        }

        Vector3 GetHoverPosition()
        {
            Vector3 maxPosition = _originPosition + transform.up * minMaxYMovement.y;
            Vector3 minPosition = _originPosition + transform.up * minMaxYMovement.x;

            if (Vector3.Distance(transform.position, minPosition) < distanceToChangeDirection)
                _shouldGoDown = false;
            else if (Vector3.Distance(transform.position, maxPosition) < distanceToChangeDirection)
                _shouldGoDown = true;

            return Vector3.Lerp(
                transform.position,
                _shouldGoDown ? minPosition : maxPosition,
                Time.deltaTime);
        }
    }
}
