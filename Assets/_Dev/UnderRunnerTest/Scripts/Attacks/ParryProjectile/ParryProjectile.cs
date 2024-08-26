using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Enemy;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ParryProjectile;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.ParryProjectile
{
    public class ParryProjectile : MonoBehaviour, IDeflectable
    {
        [Header("First Force Properties")]
        [SerializeField] private Vector3 startForce;
        [SerializeField] private Vector3 angularVelocityForce;
        [SerializeField] private float secondsInAngularVelocity;

        [Header("Second Force Properties")] 
        [SerializeField] private float secondForceAcceleration;
        [SerializeField] private float secondsInFollowForce;
        
        [Header("Damage properties")]
        [SerializeField] private int damage = 5;

        [Header("Parry Properties")] [SerializeField]
        private float startVelocityInParry;
        
        private GameObject _objectToFollow;
        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            
            _rigidbody.AddForce(startForce, ForceMode.Impulse);
            
            StartCoroutine(ApplyAngularForce());
        }

        private IEnumerator ApplyAngularForce()
        {
            float timeApplyingAngularForce = 0f;

            while (timeApplyingAngularForce < secondsInAngularVelocity)
            {
                _rigidbody.AddForce(angularVelocityForce, ForceMode.Acceleration);

                timeApplyingAngularForce += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            StartCoroutine(ApplyFollowForce());
        }

        private IEnumerator ApplyFollowForce()
        {
            float timeApplyingFollowForce = 0f;

            while (timeApplyingFollowForce < secondsInFollowForce)
            {
                Vector3 direction = GetDirection(_objectToFollow.gameObject.transform.position);              
                _rigidbody.AddForce(direction * angularVelocityForce.magnitude, ForceMode.Acceleration);

                timeApplyingFollowForce += Time.fixedDeltaTime;
                
                yield return new WaitForFixedUpdate();
            }
            
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponentInChildren<EnemyController>();

                enemy.HandleShield(false);
                gameObject.SetActive(false);
                return;
            } 
            
            if (other.CompareTag("Player"))
            {
                Debug.Log("PLAYER NOT PARRY");
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                
                damageTaker.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }

        public void SetObjectToFollow(GameObject newObjectToFollow)
        {
            _objectToFollow = newObjectToFollow;
        }

        public void Deflect(GameObject newObjectToFollow)
        {
            _rigidbody.velocity = GetDirection(newObjectToFollow.transform.position) * startVelocityInParry;
            
            SetObjectToFollow(newObjectToFollow);
        }

        private Vector3 GetDirection(Vector3 to)
        {
            return (to - gameObject.transform.position).normalized;
        }
    }
}
