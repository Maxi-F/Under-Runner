using Events;
using UnityEngine;

namespace ObstacleSystem
{
    public class ObstacleEventCaller : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onEvent;

        public void HandleEvent()
        {
            onEvent?.RaiseEvent(gameObject);
        }
    }
}
