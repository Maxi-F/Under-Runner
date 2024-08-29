using UnityEngine;
using UnityEngine.Events;

namespace _Dev.UnderRunnerTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Events/Vector3 Channel")]
    public class Vector3EventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<Vector3> onVectorEvent;

        public void RaiseEvent(Vector3 value)
        {
            if (onVectorEvent != null)
            {
                onVectorEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}
