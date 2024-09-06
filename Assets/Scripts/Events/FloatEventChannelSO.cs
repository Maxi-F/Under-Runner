using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Float Channel")]
    public class FloatEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<float> onFloatEvent;

        public void RaiseEvent(float value)
        {
            if (onFloatEvent != null)
            {
                onFloatEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}