using System;
using System.Collections;
using System.Linq;
using Events;
using Events.ScriptableObjects;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace LevelManagement.Sequences
{
    public class StartLevelSequence : MonoBehaviour
    {
        [SerializeField] private GameObject[] otherPlayers;
        [SerializeField] private GameObject player;
        [SerializeField] private float playersVelocity;
        [SerializeField] private float otherPlayersEndZPosition;
        [SerializeField] private float playerInitZPosition;

        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onStartCinematicCanvas;
        [SerializeField] private VoidEventChannelSO onEndCinematicCanvas;
        [SerializeField] private VoidEventChannelSO onCinematicCanvasFinishedAnimation;
        [SerializeField] private BoolEventChannelSO onGameplayUICanvasEvent;
        [SerializeField] private BoolEventChannelSO onCinematicUICanvasEvent;
        [SerializeField] private VoidEventChannelSO onCinematicEnded;
        
        private bool _isCinematicCanvasAnimating;

        private void OnEnable()
        {
            onCinematicCanvasFinishedAnimation?.onEvent.AddListener(HandleFinishedAnimation);
        }

        private void OnDisable()
        {
            onGameplayUICanvasEvent?.RaiseEvent(false);
        }

        public Sequence GetStartSequence()
        {
            Sequence startSequence = new Sequence();
            
            startSequence.AddPreAction(StopPlayerMovement());
            startSequence.AddPreAction(HandleStartCinematicCanvas());
            startSequence.AddPreAction(MoveOtherPlayers());
            startSequence.SetAction(MovePlayerToMiddle());
            startSequence.AddPostAction(HandleStopCinematicCanvas());

            return startSequence;
        }

        private IEnumerator StopPlayerMovement()
        {
            onCinematicStarted?.RaiseEvent();
            PlayerAgent agent = player.GetComponent<PlayerAgent>();
            
            agent.DisableFSM();
            yield return null;
        }

        private IEnumerator MovePlayerToMiddle()
        {
            while (player.transform.position.z < playerInitZPosition)
            {
                player.transform.position += new Vector3(0, 0, playersVelocity) * Time.deltaTime;
                yield return null;
            }
            
            PlayerAgent agent = player.GetComponent<PlayerAgent>();
            agent.EnableFSM();
        }

        private IEnumerator MoveOtherPlayers()
        {
            foreach (var otherPlayer in otherPlayers)
            {
                otherPlayer.SetActive(true);
            }

            while (otherPlayers.Any(player => player.activeInHierarchy))
            {
                foreach (var otherPlayer in otherPlayers)
                {
                    otherPlayer.transform.position += new Vector3(0, 0, playersVelocity) * Time.deltaTime;

                    if (otherPlayer.transform.position.z > otherPlayersEndZPosition)
                    {
                        otherPlayer.SetActive(false);
                    }
                }

                yield return null;
            }
        }

        private IEnumerator HandleStopCinematicCanvas()
        {
            onEndCinematicCanvas?.RaiseEvent();
            _isCinematicCanvasAnimating = true;
            yield return new WaitWhile(() => _isCinematicCanvasAnimating);
            onGameplayUICanvasEvent?.RaiseEvent(true);
            onCinematicUICanvasEvent?.RaiseEvent(false);
            onCinematicEnded?.RaiseEvent();
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
