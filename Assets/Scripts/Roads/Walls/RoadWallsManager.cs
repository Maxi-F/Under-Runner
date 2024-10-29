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
            GameObject wall;
            switch (wallType)
            {
                case WallTypes.Corner:
                    wall = CornerWallsObjectPool.Instance.GetRandomPooledObject();
                    wall.SetActive(true);
                    wallsToReturn.Add(wall);
                    wall = CornerWallsObjectPool.Instance.GetRandomPooledObject();
                    wallsToReturn.Add(wall);
                    break;
                case WallTypes.Middle:
                    wall = MiddleWallsObjectPool.Instance.GetRandomPooledObject();
                    wall.SetActive(true);
                    wallsToReturn.Add(wall);
                    wall = MiddleWallsObjectPool.Instance.GetRandomPooledObject();
                    wallsToReturn.Add(wall);
                    break;
            }

            return wallsToReturn;
        }
    }
}