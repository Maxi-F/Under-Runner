using Events;
using UnityEngine;

namespace Roads
{
    public class DeleteOnPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToDeleteOn;
        [SerializeField] private GameObject roadObject;

        [Header("Events")] [SerializeField] private GameObjectEventChannelSO onDeleteRoadEvent;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.z < pointToDeleteOn.z)
            {
                RoadObjectPool.Instance?.ReturnToPool(roadObject);
                onDeleteRoadEvent?.RaiseEvent(roadObject);
            }
        }
    }
}
