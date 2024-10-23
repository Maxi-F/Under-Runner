using System;
using Events.ScriptableObjects;
using UnityEngine;

namespace Managers.Pause
{
    public class PauseOpenHandler : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO onPauseHandleEvent;
        [SerializeField] private BoolEventChannelSO onPauseCanvasHandleEvent;
        private void OnEnable()
        {
            onPauseHandleEvent?.onTypedEvent.AddListener(HandlePauseCanvas);
        }

        private void OnDisable()
        {
            onPauseHandleEvent?.onTypedEvent.RemoveListener(HandlePauseCanvas);
        }

        private void HandlePauseCanvas(bool value)
        {
            onPauseCanvasHandleEvent?.RaiseEvent(value);
        }
    }
}
