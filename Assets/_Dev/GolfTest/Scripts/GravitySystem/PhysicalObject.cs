using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Dev.GolfTest.Scripts.GravitySystem
{
    public class PhysicalObject : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        private Rigidbody _rb;
        private Vector3 currentVelocity;

        public Vector3 CurrentVelocity => currentVelocity;
        #region Constants

        private const float GRAVITY_FORCE = 6.74E-5F;

        #endregion

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            currentVelocity = initialVelocity;
        }

        private void FixedUpdate()
        {
            foreach (CelestialBody celestialBody in FindObjectsOfType<CelestialBody>())
            {
                UpdateVelocity(celestialBody);
            }

            UpdatePosition();
            
            Debug.Log($"RigidBody Velocity: {_rb.velocity}");
        }

        private void UpdateVelocity(CelestialBody body)
        {
            Vector3 bodyPosition = body.transform.position;
            Vector3 thisPosition = _rb.position;
            float sqrDistance = (bodyPosition - thisPosition).sqrMagnitude;
            Vector3 forceDir = (bodyPosition - thisPosition).normalized;
            float force = GRAVITY_FORCE * _rb.mass * (body.Mass * 1000) / sqrDistance;
            currentVelocity += forceDir * (force * Time.deltaTime);
        }

        private void UpdatePosition()
        {
            _rb.position += currentVelocity * Time.deltaTime;
        }

        private void OnCollisionStay(Collision other)
        {
            currentVelocity = Vector3.zero;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Atmosphere"))
                return;

            currentVelocity -= currentVelocity * (other.transform.parent.GetComponent<CelestialBody>().AtmosphereDrag * Time.fixedDeltaTime);
        }
    }
}