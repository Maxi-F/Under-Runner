using System;
using Events;
using UnityEngine;

namespace Player
{
    public class NewRoadTrigger : MonoBehaviour
    {
        public VoidEventChannelSO onNewRoadTriggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("NewRoad"))
            {
                onNewRoadTriggerEvent?.RaiseEvent();
            }
        }
    }
}
