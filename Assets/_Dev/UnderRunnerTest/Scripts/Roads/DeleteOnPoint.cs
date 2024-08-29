using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads
{
    public class DeleteOnPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToDeleteOn;
        [SerializeField] private GameObject roadObject;

        [Header("Events")] [SerializeField] private VoidEventChannelSO onDeleteRoadEvent;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.z < pointToDeleteOn.z)
            {
                Destroy(roadObject);
                onDeleteRoadEvent?.RaiseEvent();
            }
        }
    }
}
