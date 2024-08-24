using System;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Player
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
