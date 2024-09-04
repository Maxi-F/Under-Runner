using System;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.LevelManagement
{
    [Serializable]
    public class ObstacleData
    {
        public float obstaclesDuration;
        public float obstacleCooldown;
    }

    [Serializable]
    public class RoadData
    {
        public Vector3 roadVelocity;
    }
    
    [CreateAssetMenu(menuName = "Level Loop Config", fileName = "LevelLoopConfig", order = 0)]
    public class LevelLoopSO : ScriptableObject
    {
        public ObstacleData obstacleData;
        public RoadData roadData;
    }
}
