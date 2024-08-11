using System;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.GravitySystem
{
    public class CelestialBody : MonoBehaviour
    {
        [Header("Celestial Body Properties")]
        
        [SerializeField] private float size;
        [SerializeField] private float mass;

        public float Size
        {
            get { return size; }
        }
        public float Mass
        {
            get { return mass; }
        }

        private void OnValidate()
        {
            transform.localScale = new Vector3(size,size,size);
        }
    }
}