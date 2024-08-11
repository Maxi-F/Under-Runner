using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Dev.GolfTest.Scripts.GravitySystem
{
    public class PhysicalObject : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector3 _velocity;

        #region Constants

        private const float GRAVITY_FORCE = 6.74E-5F;

        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            foreach (CelestialBody celestialBody in FindObjectsOfType<CelestialBody>())
            {
                UpdateVelocity(celestialBody);
            }
        }

        private void FixedUpdate()
        {
            rb.AddForce(_velocity);
        }

        private void UpdateVelocity(CelestialBody body)
        {
            Vector3 bodyPosition = body.transform.position;
            Vector3 thisPosition = transform.position;
            float sqrDistance = (bodyPosition - thisPosition).sqrMagnitude;
            Vector3 forceDir = (bodyPosition - thisPosition).normalized;
            float force = GRAVITY_FORCE * ((body.Mass * rb.mass) / sqrDistance);
            _velocity += forceDir * force;
        }

        private void OnCollisionStay(Collision other)
        {
            _velocity = Vector3.zero;
        }
    }
}