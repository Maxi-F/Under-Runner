using System;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Vector3 velocity;
        [SerializeField] private Transform road;
        
        private void Update()
        {
            road.position += velocity * Time.deltaTime;
        }
    }
}
