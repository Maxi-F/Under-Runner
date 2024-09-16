using System;
using UnityEngine;

namespace MapBounds
{
    [CreateAssetMenu(fileName = "MapBounds", menuName = "Map/MapBounds", order = 0)]
    public class MapBoundsSO : ScriptableObject
    {
        public AxisBounds horizontalBounds;
        public AxisBounds depthBounds;
    }
}