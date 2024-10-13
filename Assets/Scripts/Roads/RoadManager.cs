using System;
using System.Collections.Generic;
using System.Linq;
using Events;
using Roads.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Roads
{
    public class RoadManager : MonoBehaviour
    {
        [SerializeField] private GameObject startingLastRoad;
        [SerializeField] private int initRoadCount = 7;
        [SerializeField] private int maxRoads = 7;
        [SerializeField] private Vector3 roadsInitVelocity = new Vector3(0f, 0f, -20f);
        [SerializeField] private List<GameObject> initRoads;
        
        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onRoadDeleteTriggerEvent;
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private Vector3EventChannelSO onNewVelocityEvent;
        [SerializeField] private VoidEventChannelSO onGameplayEndEvent;
        
        private int _roadCount;
        private List<GameObject> _roads = new List<GameObject>();
        private Vector3 _roadsVelocity;
        private GameObject _lastRoad;

        public void OnEnable()
        {
            _roadsVelocity = roadsInitVelocity;
            _roadCount = initRoadCount;
            _lastRoad = startingLastRoad;
            _roads = new List<GameObject>();
            
            foreach (var initRoad in initRoads)
            {
                initRoad.SetActive(true);
                _roads.Add(initRoad);
            }
            onRoadDeleteTriggerEvent?.onGameObjectEvent.AddListener(HandleDeleteRoad);
            onGameplayEndEvent?.onEvent.AddListener(HandleRemoveRoads);
        }

        public void OnDisable()
        {
            onRoadDeleteTriggerEvent?.onGameObjectEvent.RemoveListener(HandleDeleteRoad);
            onGameplayEndEvent?.onEvent.RemoveListener(HandleRemoveRoads);
        }

        private void HandleRemoveRoads()
        {
            foreach (var road in _roads.ToList())
            {
                _roads.Remove(road);
                RoadObjectPool.Instance?.ReturnToPool(road);
            }
        }

        public void HandleNewVelocity(Vector3 velocity)
        {
            _roadsVelocity = velocity;
            onNewVelocityEvent?.RaiseEvent(velocity);
        }

        private void HandleDeleteRoad(GameObject road)
        {
            _roadCount--;
            _roads.Remove(road);
            HandleNewRoad();
        }

        private void HandleNewRoad()
        {
            if (_roadCount > maxRoads) return;
            
            RoadEnd roadEnd = _lastRoad.GetComponentInChildren<RoadEnd>();

            GameObject newLastRoad = RoadObjectPool.Instance?.GetPooledObject();
            if (newLastRoad == null)
            {
                Debug.LogError("new last road was null!");
                return;
            }

            newLastRoad.transform.position = roadEnd.GetRoadEnd().position + _roadsVelocity * Time.deltaTime;
            newLastRoad.SetActive(true);
            _roads.Add(newLastRoad);
            
            Movement roadMovement = newLastRoad.GetComponentInChildren<Movement>();
            roadMovement.SetVelocity(_roadsVelocity);
            
            onRoadInstantiatedEvent?.RaiseEvent(newLastRoad);
            
            _lastRoad = newLastRoad;

            _roadCount++;
        }
    }
}
