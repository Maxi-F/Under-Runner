using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Roads.Walls
{
    public enum WallTypes
    {
        Corner,
        Middle
    }

    public class RoadWallsManager : MonoBehaviour
    {
        public static RoadWallsManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        public List<GameObject> GetRandomWalls(WallTypes wallType)
        {
            List<GameObject> wallsToReturn = new List<GameObject>();
            switch (wallType)
            {
                case WallTypes.Corner:
                    wallsToReturn.Add(CornerWallsObjectPool.Instance.GetRandomPooledObject());
                    wallsToReturn.Add(CornerWallsObjectPool.Instance.GetRandomPooledObject());
                    break;
                case WallTypes.Middle:
                    wallsToReturn.Add(MiddleWallsObjectPool.Instance.GetRandomPooledObject());
                    wallsToReturn.Add(MiddleWallsObjectPool.Instance.GetRandomPooledObject());
                    break;
            }

            return wallsToReturn;
        }
    }
}