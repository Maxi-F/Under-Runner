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
        [SerializeField] private int initRoadCount = 7;
        [SerializeField] private int maxRoads = 7;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onNewRoadTriggerEvent;

        [SerializeField] private VoidEventChannelSO onRoadDeleteTriggerEvent;

        private int _roadCount;
        private int _actualIndex = 0;
        private GameObject _lastRoad;

        public void Start()
        {
            _roadCount = initRoadCount;
            _lastRoad = startingLastRoad;
            onNewRoadTriggerEvent?.onEvent.AddListener(HandleNewRoad);
            onRoadDeleteTriggerEvent?.onEvent.AddListener(HandleDeleteRoad);
        }

        public void OnDisable()
        {
            onNewRoadTriggerEvent?.onEvent.RemoveListener(HandleNewRoad);
            onRoadDeleteTriggerEvent?.onEvent.RemoveListener(HandleDeleteRoad);
        }

        private void HandleDeleteRoad()
        {
            _roadCount--;
        }

        private void HandleNewRoad()
        {
            if (_roadCount > maxRoads) return;
            
            RoadEnd roadEnd = _lastRoad.GetComponentInChildren<RoadEnd>();
            
            GameObject newLastRoad = Instantiate(roads[_actualIndex].roadSection, roadEnd.transform.position,
                roads[_actualIndex].startRotation);

            _lastRoad = newLastRoad;
            
            _actualIndex++;

            if (_actualIndex >= roads.Length) _actualIndex = 0;

            _roadCount++;
        }
    }
}
