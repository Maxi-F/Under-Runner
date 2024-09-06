using System;
using Events;
using Roads.ScriptableObjects;
using UnityEngine;

namespace Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private RoadSO[] roads;
        [SerializeField] private GameObject startingLastRoad;
        [SerializeField] private int initRoadCount = 7;
        [SerializeField] private int maxRoads = 7;
        [SerializeField] private Vector3 roadsInitVelocity = new Vector3(0f, 0f, -20f);
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onNewRoadTriggerEvent;
        [SerializeField] private VoidEventChannelSO onRoadDeleteTriggerEvent;
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private Vector3EventChannelSO onNewVelocityEvent;
        
        private int _roadCount;
        private int _actualIndex = 0;
        private Vector3 _roadsVelocity;
        private GameObject _lastRoad;

        public void OnEnable()
        {
            _roadsVelocity = roadsInitVelocity;
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

        public void HandleNewVelocity(Vector3 velocity)
        {
            _roadsVelocity = velocity;
            onNewVelocityEvent?.RaiseEvent(velocity);
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

            Movement roadMovement = newLastRoad.GetComponentInChildren<Movement>();
            roadMovement.SetVelocity(_roadsVelocity);
            
            onRoadInstantiatedEvent?.RaiseEvent(newLastRoad);
            
            _lastRoad = newLastRoad;
            
            _actualIndex++;

            if (_actualIndex >= roads.Length) _actualIndex = 0;

            _roadCount++;
        }
    }
}
