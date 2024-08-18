using System;
using _Dev.GolfTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Roads.ScriptableObjects;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private RoadSO[] roads;
        [SerializeField] private VoidEventChannelSO onNewRoadTriggerEvent;
        
        private int _actualIndex = 0;

        public void Start()
        {
            onNewRoadTriggerEvent.onEvent.AddListener(HandleNewRoad);
        }

        public void OnDisable()
        {
            onNewRoadTriggerEvent.onEvent.RemoveListener(HandleNewRoad);
        }

        private void HandleNewRoad()
        {
            Instantiate(roads[_actualIndex].roadSection, roads[_actualIndex].distanceToSpawnTo,
                roads[_actualIndex].startRotation);

            _actualIndex++;

            if (_actualIndex >= roads.Length) _actualIndex = 0;
        }
    }
}
