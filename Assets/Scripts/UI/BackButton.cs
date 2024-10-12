using System;
using Events;
using Events.ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class BackButton : MonoBehaviour
    {
        [SerializeField] private BoolChannelChannelSO onChangeBoolChannel;
        [SerializeField] private BoolEventRaiser boolEventRaiser;

        private void Awake()
        {
            onChangeBoolChannel?.onTypedEvent.AddListener(HandleChangeChannel);
        }

        private void OnDestroy()
        {
            onChangeBoolChannel?.onTypedEvent.RemoveListener(HandleChangeChannel);
        }

        private void HandleChangeChannel(BoolEventChannelSO eventChannel)
        {
            boolEventRaiser.ChangeEvent(eventChannel);
        }
    }
}
