using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Dev.UnderRunnerTest.Scripts.Events
{
    [CreateAssetMenu(menuName = "Events/GameObject Channel")]
    public class GameObjectEventChannelSO : VoidEventChannelSO
    {
        public UnityEvent<GameObject> onGameObjectEvent;

        public void RaiseEvent(GameObject value)
        {
            if (onGameObjectEvent != null)
            {
                onGameObjectEvent.Invoke(value);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}