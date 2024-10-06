using System;
using System.Collections;
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
        
        private SceneryManager _sceneryManager;
        
        private void OnEnable()
        {
            _sceneryManager = FindObjectOfType<SceneryManager>();
            if (_sceneryManager == null)
            {
                Debug.Log("Scenery manager is null. Will not load optional scenes.");
                return;
            }
            
            foreach (var optionalScene in optionalScenes)
            {
                _sceneryManager?.LoadScene(optionalScene);
            }

            if (setAsActiveOnBoot)
            {
                StartCoroutine(SetSceneAsActiveScene());
            }
            
            SubscribeToActions();
        }

        private void OnDisable()
        {
            if (_sceneryManager == null)
            {
                Debug.Log("Scenery manager is null. Will not unsubscribe from actions.");
                return;
            }
            UnsubscribeToActions();
        }

        private IEnumerator SetSceneAsActiveScene()
        {
            // returns a yield null as it needs a frame to load the scene, then it can be set
            // as active.
            yield return null;
            _sceneryManager?.SetActiveScene(sceneName);
        }
        
        /// <summary>
        /// Subscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void SubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                Debug.Log(aSceneName);
                _sceneryManager?.SubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        /// <summary>
        /// Unsubscribes to add scene event of the scenes to subscribe to.
        /// </summary>
        private void UnsubscribeToActions()
        {
            Array.ForEach(scenesToSubscribeTo, (aSceneName) =>
            {
                _sceneryManager?.UnsubscribeEventToAddScene(aSceneName, UnloadScene);
            });
        }
        
        /// <summary>
        /// Unloads current scene and optional scenes.
        /// </summary>
        private void UnloadScene()
        {
            _sceneryManager?.UnloadScene(sceneName);

            foreach (var optionalScene in optionalScenes)
            {
                _sceneryManager?.UnloadScene(optionalScene);
            }
        }
    }
}
