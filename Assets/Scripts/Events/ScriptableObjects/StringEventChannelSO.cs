using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/String Channel")]
    public class StringEventChannelSo : VoidEventChannelSO
    {
        [FormerlySerializedAs("onIntEvent")] public UnityEvent<string> onStringEvent;

        public void RaiseEvent(string value)
        {
            if (onStringEvent != null)
            {
                onStringEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}