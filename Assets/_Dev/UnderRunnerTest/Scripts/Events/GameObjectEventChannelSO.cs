using UnityEngine;
using UnityEngine.Events;

namespace _Dev.UnderRunnerTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Events/GameObject Channel")]
    public class GameObjectEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<GameObject> onIntEvent;

        public void RaiseEvent(GameObject value)
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
