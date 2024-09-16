using System;
using UnityEngine;

namespace MapBounds
{
    [CreateAssetMenu(fileName = "MapBounds", menuName = "Map/MapBounds", order = 0)]
    public class MapBoundsSO : ScriptableObject
    {
        public AxisBounds horizontalBounds;
        public AxisBounds depthBounds;

        public Vector3 ClampPosition(Vector3 position)
        {
            float xClamp = Mathf.Clamp(position.x, horizontalBounds.min, horizontalBounds.max);
            float zClamp = Mathf.Clamp(position.z, depthBounds.min, depthBounds.max);
            return new Vector3(xClamp, position.y, zClamp);
        }

        public Vector3 ClampPosition(Vector3 position, Vector3 colliderBounds)
        {
            colliderBounds *= 0.5f;
            float xClamp = Mathf.Clamp(position.x, horizontalBounds.min + colliderBounds.x, horizontalBounds.max - colliderBounds.x);
            float zClamp = Mathf.Clamp(position.z, depthBounds.min + colliderBounds.z, depthBounds.max - colliderBounds.z);
            return new Vector3(xClamp, position.y, zClamp);
        }
    }
}