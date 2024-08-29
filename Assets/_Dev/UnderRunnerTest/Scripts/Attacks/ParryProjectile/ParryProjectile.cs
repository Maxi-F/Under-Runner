using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Enemy;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ParryProjectile;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.ParryProjectile
{
    [Serializable]
    public class ParryProjectileFirstForce
    {
        public Vector3 startImpulse;
        public Vector3 angularForce;
        public float secondsInAngularVelocity;
    }
    
    public class ParryProjectile : MonoBehaviour, IDeflectable
    {
        [Header("Second Force Properties")]
        [SerializeField] private float secondForceAcceleration;
        [SerializeField] private float secondsInFollowForce;
        [SerializeField] private float yConstantForce = 0.25f;
        
        [Header("Damage properties")]
        [SerializeField] private int damage = 5;

        [SerializeField] private int shieldDamage = 1;

        [Header("Parry Properties")] [SerializeField]
        private float startVelocityInParry;

        private GameObject _firstObjectToFollow;
        private GameObject _objectToFollow;
        private Rigidbody _rigidbody;
        private ParryProjectileFirstForce _parryProjectileConfig;

        private float _timeApplyingFollowForce = 0f;
        private bool _isStarted = false;
        private float _followForceVelocity;

        public void SetFirstForce(ParryProjectileFirstForce config)
        {
            _parryProjectileConfig = config;
        }
        
        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
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

            _followForceVelocity = _rigidbody.velocity.magnitude;
            Debug.Log(_followForceVelocity);
            StartCoroutine(ApplyFollowForce());
        }

        private IEnumerator ApplyFollowForce()
        {
            while (_timeApplyingFollowForce < secondsInFollowForce)
            {
                Vector3 direction = GetDirection(_objectToFollow.gameObject.transform.position);
                _rigidbody.velocity = direction * _followForceVelocity * Time.deltaTime;

                _timeApplyingFollowForce += Time.deltaTime;
                _followForceVelocity += secondForceAcceleration * Time.deltaTime;

                
                yield return null;
            }
            
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponentInChildren<EnemyController>();

                if (enemy.TryDestroyShield(shieldDamage))
                {
                    gameObject.SetActive(false);
                    return;
                };

                Deflect(_firstObjectToFollow);
                return;
            } 
            
            if (other.CompareTag("Player"))
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                
                damageTaker.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }

        public void SetObjectToFollow(GameObject newObjectToFollow)
        {
            _objectToFollow = newObjectToFollow;
        }
        
        public void SetFirstObjectToFollow(GameObject newFirstObjectToFollow)
        {
            _firstObjectToFollow = newFirstObjectToFollow;
        }

        public void Deflect(GameObject newObjectToFollow)
        {
            _timeApplyingFollowForce = 0f;
            _rigidbody.velocity = GetDirection(newObjectToFollow.transform.position) * startVelocityInParry;
            
            SetObjectToFollow(newObjectToFollow);
        }

        private Vector3 GetDirection(Vector3 to)
        {
            Vector3 direction = (to - gameObject.transform.position).normalized;
            
            return direction;
        }

        private void OnDrawGizmos()
        {
            if(_rigidbody != null)
                Gizmos.DrawLine(transform.position, _rigidbody.velocity);
        }
    }
}
