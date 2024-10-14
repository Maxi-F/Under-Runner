using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events.ScriptableObjects
{
    [Serializable]
    public class SubscribeToSceneData
    {
        public string sceneName;
        public UnityAction SubscribeToSceneAction;
    }
    
    [CreateAssetMenu(menuName = "Events/Subscribe To Scene Channel")]
    public class SubscribeToSceneChannelSO : VoidEventChannelSO
    {
        public UnityEvent<SubscribeToSceneData> onSubscribeEvent;
        
        public void RaiseEvent(SubscribeToSceneData data)
        {
            if (onSubscribeEvent != null)
            {
                onSubscribeEvent.Invoke(data);
            }
            else
            {
                LogNullEventError();
            }
        }
    }
}
