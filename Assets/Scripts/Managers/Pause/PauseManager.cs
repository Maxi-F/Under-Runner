using Events.ScriptableObjects;
using Input;
using UnityEngine;

namespace Managers.Pause
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private BoolEventChannelSO onHandlePauseEvent;
        [SerializeField] private PauseSO pauseData;
        
        private float _lastTimeScale;
        private void OnEnable()
        {
            pauseData.isPaused = false;
            onHandlePauseEvent?.onTypedEvent.AddListener(HandlePause);
            inputHandler?.onPauseToggle.AddListener(HandlePause);
        }

        private void OnDisable()
        {
            onHandlePauseEvent?.onTypedEvent.RemoveListener(HandlePause);
            inputHandler?.onPauseToggle.RemoveListener(HandlePause);
        }

        private void HandlePause(bool value)
        {
            pauseData.isPaused = value;

            if (pauseData.isPaused)
            {
                _lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = _lastTimeScale;
            }
        }

        private void HandlePause()
        {
            if(!pauseData.isPaused)
                onHandlePauseEvent.RaiseEvent(true);
        }
    }
}
