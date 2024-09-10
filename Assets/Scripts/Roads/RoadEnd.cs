using UnityEngine;

namespace Roads
{
    public class RoadEnd : MonoBehaviour
    {
        [SerializeField] private Transform roadEnd;

        public Transform GetRoadEnd()
        {
            return roadEnd;
        }
    }
}
