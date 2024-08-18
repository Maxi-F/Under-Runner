using System;
using _Dev.GolfTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Roads.ScriptableObjects;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private RoadSO[] roads;
        [SerializeField] private GameObject startingLastRoad;
        [SerializeField] private VoidEventChannelSO onNewRoadTriggerEvent;
        
        private int _actualIndex = 0;
        private GameObject _lastRoad;

        public void Start()
        {
            _lastRoad = startingLastRoad;
            onNewRoadTriggerEvent.onEvent.AddListener(HandleNewRoad);
        }

        public void OnDisable()
        {
            onNewRoadTriggerEvent.onEvent.RemoveListener(HandleNewRoad);
        }

        private void HandleNewRoad()
        {
            RoadEnd roadEnd = _lastRoad.GetComponentInChildren<RoadEnd>();
            
            GameObject newLastRoad = Instantiate(roads[_actualIndex].roadSection, roadEnd.transform.position,
                roads[_actualIndex].startRotation);

            _lastRoad = newLastRoad;
            
            _actualIndex++;

            if (_actualIndex >= roads.Length) _actualIndex = 0;
        }
    }
}
