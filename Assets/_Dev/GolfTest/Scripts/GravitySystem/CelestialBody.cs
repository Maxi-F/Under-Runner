using System;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.GravitySystem
{
    public class CelestialBody : MonoBehaviour
    {
        [Header("Celestial Body Properties")] [SerializeField]
        private float size;

        [Tooltip("Body's Mass In tons")] [SerializeField]
        private float mass;

        [SerializeField] private float atmosphereDrag;

        public float Size
        {
            get { return size; }
        }

        public float Mass
        {
            get { return mass; }
        }

        public float AtmosphereDrag
        {
            get { return atmosphereDrag; }
        }

        private void OnValidate()
        {
            transform.localScale = new Vector3(size, size, size);
        }
    }
}