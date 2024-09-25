using System;
using System.Collections;
using Enemy;
using Health;
using ParryProjectile;
using UnityEngine;

namespace Attacks.ParryProjectile
{
    [Serializable]
    public class ParryProjectileFirstForce
    {
        public Vector3 startImpulse;
        public Vector3 angularForce;
        public Vector3 finalPosition;
        public float secondsInAngularVelocity;
    }

    public class ParryBomb : MonoBehaviour, IDeflectable
    {
        [Header("Second Force Properties")]
        [SerializeField] private float secondForceAcceleration;

        [SerializeField] private float secondsInFollowForce;

        [Header("Damage properties")]
        [SerializeField] private int damage = 5;

        [SerializeField] private int shieldDamage = 1;

        [Header("Parry Properties")] [SerializeField]
        private float startVelocityInParry;

        private GameObject _firstObjectToFollow;
        private Vector3 _targetPosition;
        private bool _isTargetingPlayer;
        private GameObject _objectToFollow;
        private Rigidbody _rigidbody;
        private ParryProjectileFirstForce _parryProjectileConfig;

        private float _timeApplyingFollowForce = 0f;
        private bool _isStarted = false;
        private float _followForceVelocity;

        private Vector3 _direction;

        public void SetFirstForce(ParryProjectileFirstForce config)
        {
            _parryProjectileConfig = config;
            SetTargetPosition(config.finalPosition);
        }

        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            _isTargetingPlayer = true;
        }

        private void FixedUpdate()
        {
            if (!_isStarted)
            {
                StartCoroutine(ApplyAngularForce());
                _isStarted = true;
            }
        }

        private IEnumerator ApplyAngularForce()
        {
            _rigidbody.AddForce(_parryProjectileConfig.startImpulse, ForceMode.Impulse);

            float timeApplyingAngularForce = 0f;

            while (timeApplyingAngularForce < _parryProjectileConfig.secondsInAngularVelocity)
            {
                _rigidbody.AddForce(_parryProjectileConfig.angularForce, ForceMode.Acceleration);

                timeApplyingAngularForce += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            _followForceVelocity = (_rigidbody.velocity.magnitude * Time.deltaTime) / Time.fixedDeltaTime;
            StartCoroutine(ApplyTargetedForce(_targetPosition));
        }

        private IEnumerator ApplyTargetedForce(Vector3 targetPosition)
        {
            SetDirection(_targetPosition);

            while (_timeApplyingFollowForce < secondsInFollowForce)
            {
                // transform.position = Vector3.MoveTowards(transform.position, _objectToFollow.gameObject.transform.position, _followForceVelocity * Time.deltaTime);
                transform.position += _direction * (_followForceVelocity * Time.deltaTime);

                _timeApplyingFollowForce += Time.deltaTime;
                _followForceVelocity += secondForceAcceleration * Time.deltaTime;

                yield return null;
            }

            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") && !_isTargetingPlayer)
            {
                EnemyController enemy = other.GetComponentInChildren<EnemyController>();

                if (enemy.TryDestroyShield(shieldDamage))
                {
                    gameObject.SetActive(false);
                    return;
                }

                Deflect(_firstObjectToFollow);
                return;
            }

            if (other.CompareTag("Player") && _isTargetingPlayer)
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();

                damageTaker.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }

        public void SetObjectToFollow(GameObject newObjectToFollow)
        {
            _objectToFollow = newObjectToFollow;
            _isTargetingPlayer = false;
        }

        public void SetFirstObjectToFollow(GameObject newFirstObjectToFollow)
        {
            _firstObjectToFollow = newFirstObjectToFollow;
            _isTargetingPlayer = true;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position;
        }

        public void Deflect(GameObject newObjectToFollow)
        {
            _timeApplyingFollowForce = 0f;
            _rigidbody.velocity = GetDirection(newObjectToFollow.transform.position) * startVelocityInParry;

            _isTargetingPlayer = !_isTargetingPlayer;
            SetTargetPosition(newObjectToFollow.transform.position);
            SetDirection(_targetPosition);
        }

        private void SetDirection(Vector3 position)
        {
            _direction = GetDirection(position);
        }

        private Vector3 GetDirection(Vector3 to)
        {
            Vector3 direction = (to - gameObject.transform.position).normalized;

            return direction;
        }

        private void OnDrawGizmos()
        {
            if (_rigidbody != null)
                Gizmos.DrawLine(transform.position, _rigidbody.velocity);
        }
    }
}