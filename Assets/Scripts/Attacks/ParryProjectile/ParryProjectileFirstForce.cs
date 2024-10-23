using System;
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
}