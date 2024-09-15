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
        [SerializeField] private GameObjectEventChannelSO onRoadDeleteTriggerEvent;
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private Vector3EventChannelSO onNewVelocityEvent;
        
        private int _roadCount;
        private Vector3 _roadsVelocity;
        private GameObject _lastRoad;

        public void OnEnable()
        {
            _roadsVelocity = roadsInitVelocity;
            _roadCount = initRoadCount;
            _lastRoad = startingLastRoad;
            onRoadDeleteTriggerEvent?.onGameObjectEvent.AddListener(HandleDeleteRoad);
        }

        public void OnDisable()
        {
            onRoadDeleteTriggerEvent?.onGameObjectEvent.RemoveListener(HandleDeleteRoad);
        }

        public void HandleNewVelocity(Vector3 velocity)
        {
            _roadsVelocity = velocity;
            onNewVelocityEvent?.RaiseEvent(velocity);
        }

        private void HandleDeleteRoad(GameObject road)
        {
            _roadCount--;
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
            
            Movement roadMovement = newLastRoad.GetComponentInChildren<Movement>();
            roadMovement.SetVelocity(_roadsVelocity);
            
            onRoadInstantiatedEvent?.RaiseEvent(newLastRoad);
            
            _lastRoad = newLastRoad;

            _roadCount++;
        }
    }
}
