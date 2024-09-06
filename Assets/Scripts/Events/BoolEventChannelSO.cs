using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Bool Channel")]
    public class BoolEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<bool> onBoolEvent;

        public void RaiseEvent(bool value)
        {
            if (onBoolEvent != null)
            {
                onBoolEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}
