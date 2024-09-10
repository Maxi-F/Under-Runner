using System.Collections.Generic;
using UnityEngine;

namespace Roads.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create Roads Factory Config", fileName = "RoadsConfig", order = 0)]
    public class RoadsConfigSO : ScriptableObject
    {
        public List<RoadSO> roads;
    }
}
