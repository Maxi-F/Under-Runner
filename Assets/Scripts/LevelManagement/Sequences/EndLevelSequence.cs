using System.Collections;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using Utils;

namespace LevelManagement.Sequences
{
    public class EndLevelSequence : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float playerVelocity;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onStartCinematicCanvas;
        [SerializeField] private VoidEventChannelSO onEndCinematicCanvas;
        [SerializeField] private VoidEventChannelSO onCinematicCanvasFinishedAnimation;
        [SerializeField] private BoolEventChannelSO onGameplayUICanvasEvent;
        [SerializeField] private BoolEventChannelSO onCinematicUICanvasEvent;
        [SerializeField] private StringEventChannelSo onChangeSceneEvent;
        [SerializeField] private VoidEventChannelSO onCinematicEnded;
        
        private bool _isCinematicCanvasAnimating;
        
        private void OnEnable()
        {
            onCinematicCanvasFinishedAnimation?.onEvent.AddListener(HandleFinishedAnimation);
        }
        
        private void OnDisable()
        {
            onCinematicCanvasFinishedAnimation?.onEvent.RemoveListener(HandleFinishedAnimation);
        }

        public Sequence GetEndSequence()
        {
            Sequence endSequence = new Sequence();
            
            endSequence.AddPostAction(HandleStartCinematicCanvas());
            
            return endSequence;
        }
        

        private IEnumerator HandleStartCinematicCanvas()
        {
            onGameplayUICanvasEvent?.RaiseEvent(false);
            onCinematicUICanvasEvent?.RaiseEvent(true);
            onStartCinematicCanvas?.RaiseEvent();
            _isCinematicCanvasAnimating = true;
            yield return new WaitWhile(() => _isCinematicCanvasAnimating);
        }


        private void HandleFinishedAnimation()
        {
            _isCinematicCanvasAnimating = false;
        }
    }
}
