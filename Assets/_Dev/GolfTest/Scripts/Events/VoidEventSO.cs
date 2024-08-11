using UnityEngine;
using UnityEngine.Events;

namespace _Dev.GolfTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        protected UnityEvent onEvent;

        public void RaiseEvent()
        {
            if (onEvent != null)
            {
                onEvent.Invoke();
            }
            else
            {
                LogNullEventError();
            }
        }

        protected void LogNullEventError()
        {
            Debug.LogError($"{this.name} has no events. Please check if" +
                           $"events have been added correctly.");
        }
    }
}
