using UnityEngine;
using UnityEngine.Events;

namespace _Dev.GolfTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Events/Int Channel")]
    public class IntEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<int> onIntEvent;

        public void RaiseEvent(int value)
        {
            if (onIntEvent != null)
            {
                onIntEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}