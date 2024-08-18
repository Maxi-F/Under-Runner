using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create Road Config", fileName = "RoadConfig", order = 0)]
    public class RoadSO : ScriptableObject
    {
        public GameObject roadSection;
        public Vector3 distanceToSpawnTo;
        public Quaternion startRotation;
    }
}
