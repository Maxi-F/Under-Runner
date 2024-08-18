using UnityEngine;
using UnityEngine.Events;

namespace _Dev.GolfTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Events/Int Channel")]
    public class IntEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<int> onFloatEvent;

        public void RaiseEvent(int value)
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