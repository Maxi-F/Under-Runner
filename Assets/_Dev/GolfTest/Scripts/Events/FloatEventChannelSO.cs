using UnityEngine;
using UnityEngine.Events;

namespace _Dev.GolfTest.Scripts.Events
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