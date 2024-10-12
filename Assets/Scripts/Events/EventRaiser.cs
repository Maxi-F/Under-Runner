using Events.ScriptableObjects;
using UnityEngine;

namespace Events
{
    public class EventRaiser<T> : MonoBehaviour
    {
        [SerializeField] private EventChannelSO<T> eventToRaise;

        private EventChannelSO<T> _event;

        public void Awake()
        {
            _event = eventToRaise;
        }

        public virtual void RaiseEvent(T value)
        {
            eventToRaise?.RaiseEvent(value);
        }

        public void ChangeEvent(EventChannelSO<T> eventChannel)
        {
            _event = eventChannel;
        }
    }
}