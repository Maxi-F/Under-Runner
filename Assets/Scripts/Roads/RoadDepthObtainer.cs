using System;
using UnityEngine;

namespace Roads
{
    public class RoadDepthObtainer : MonoBehaviour
    {
        private BoxCollider _collider;

        private void OnEnable()
        {
            _collider ??= GetComponent<BoxCollider>();
        }

        public float GetRoadDepth()
        {
            return _collider.bounds.size.z;
        }
    }
}
