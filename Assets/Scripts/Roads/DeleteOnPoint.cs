using Events;
using UnityEngine;

namespace Roads
{
    public class DeleteOnPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToDeleteOn;
        [SerializeField] private GameObject roadObject;

        [Header("Events")] [SerializeField] private GameObjectEventChannelSO onDeleteRoadEvent;
        
        void Update()
        {
            if (transform.position.z < pointToDeleteOn.z)
            {
                onDeleteRoadEvent?.RaiseEvent(roadObject);
                Debug.Log($"DELETING {roadObject.GetInstanceID()}");
                RoadObjectPool.Instance?.ReturnToPool(roadObject);
            }
        }
    }
}
