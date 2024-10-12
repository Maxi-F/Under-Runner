using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using Managers;
using UnityEngine;

namespace Scenes
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private string[] scenesToSubscribeTo;
        [Tooltip("Current scene name")] [SerializeField] private string sceneName;
        [Tooltip("Optional scenes to activate with the current scene")] [SerializeField] private string[] optionalScenes = new string[] {};
        [SerializeField] private bool setAsActiveOnBoot = false;

        [Header("events")] 
        [SerializeField] private StringEventChannelSo onLoadSceneEvent;
        [SerializeField] private StringEventChannelSo onUnloadSceneEvent;
        [SerializeField] private StringEventChannelSo onSetActiveSceneEvent;
        [SerializeField] private SubscribeToSceneChannelSO onSubscribeToSceneEvent;
        [SerializeField] private SubscribeToSceneChannelSO onUnSubscribeToSceneEvent;
        
        private void OnEnable()
        {
            foreach (var optionalScene in optionalScenes)
            {
                onLoadSceneEvent?.RaiseEvent(optionalScene);
            }

            if (setAsActiveOnBoot)
            {
                StartCoroutine(SetSceneAsActiveScene());
            }
            
            SubscribeToActions();
        }

        private void OnDisable()
        {
            UnsubscribeToActions();
        }

        private IEnumerator SetSceneAsActiveScene()
        {
            // returns a yield null as it needs a frame to load the scene, then it can be set
            // as active.
            yield return null;
            onSetActiveSceneEvent?.RaiseEvent(sceneName);
        }
        
        /// <summary>
        /// Subscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void SubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                onSubscribeToSceneEvent?.RaiseEvent(new SubscribeToSceneData()
                {
                    sceneName = aSceneName,
                    SubscribeToSceneAction = UnloadScene
                });
            });
        }
        
        /// <summary>
        /// Unsubscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                onUnSubscribeToSceneEvent?.RaiseEvent(new SubscribeToSceneData()
                {
                    sceneName = aSceneName,
                    SubscribeToSceneAction = UnloadScene
                });
            });
        }
        
        /// <summary>
        /// Unloads current scene and optional scenes.
        /// </summary>
        private void UnloadScene()
        {
            onUnloadSceneEvent?.RaiseEvent(sceneName);
            
            foreach (var optionalScene in optionalScenes)
            {
                onUnloadSceneEvent?.RaiseEvent(optionalScene);
            }
        }
    }
}
