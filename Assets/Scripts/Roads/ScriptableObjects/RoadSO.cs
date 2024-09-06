using UnityEngine;

namespace Roads.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create Road Config", fileName = "RoadConfig", order = 0)]
    public class RoadSO : ScriptableObject
    {
        public GameObject roadSection;
        public Quaternion startRotation;
    }
}
