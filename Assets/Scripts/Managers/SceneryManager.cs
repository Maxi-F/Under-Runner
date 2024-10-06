using System;
using System.Collections.Generic;
using Events;
using Scenes.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneryManager : MonoBehaviour
    {
        [SerializeField] private ScenesDataConfig scenesDataConfig;

        [Tooltip("Scenes that are on boot")] 
        [SerializeField] private string[] initScenes;

        [Header("events")] [SerializeField] private StringEventChannelSo onLoadScene;
        
        private readonly List<SerializedScene> _activeScenes = new List<SerializedScene>();

        private void OnEnable()
        {
            onLoadScene?.onStringEvent.AddListener(LoadScene);
        }

        private void OnDisable()
        {
            onLoadScene?.onStringEvent.RemoveListener(LoadScene);
        }

        /// <summary>
        /// Loads all init scenes.
        /// </summary>
        public void InitScenes()
        {
            foreach (var initScene in initScenes)
            {
                LoadScene(initScene);
            }
        }

        /// <summary>
        /// Loads the scene.
        /// </summary>
        /// <param name="sceneName">The scene name to load.</param>
        public void LoadScene(string sceneName)
        {
            if (sceneName == "Exit")
            {
                Debug.Log("Quitting...");
                Application.Quit();
                return;
            }

            SerializedScene scene = scenesDataConfig.GetSerializedScene(sceneName);
            
            scene.OnLoad?.Invoke();
            AddScene(scene);
        }

        /// <summary>
        /// Unloads the scene.
        /// </summary>
        /// <param name="sceneName">The scene name to unload.</param>
        public void UnloadScene(string aSceneName)
        {
            SerializedScene aScene = scenesDataConfig.GetSerializedScene(aSceneName);
            
            if (_activeScenes.Exists(scene => scene.sceneName == aScene.sceneName))
            {
                SceneManager.UnloadSceneAsync(aScene.index);
                aScene.OnUnload?.Invoke();
                _activeScenes.RemoveAt(_activeScenes.FindIndex(scene => scene.sceneName == aScene.sceneName));
            }
            else
            {
                Debug.LogWarning($"{aScene.sceneName} not active!");
            }
        }

        /// <summary>
        /// Loads a new scene with scene mode aditive.
        /// </summary>
        /// <param name="scene">Serializable scene to load.</param>
        private void AddScene(SerializedScene scene)
        {
            SceneManager.LoadScene(scene.index, LoadSceneMode.Additive);
            _activeScenes.Add(scene);
        }

        /// <summary>
        /// Subscribes an event to an Add scene event
        /// </summary>
        /// <param name="sceneName">Scene name to load the event to</param>
        /// <param name="action">Action to subscribe.</param>
        public void SubscribeEventToAddScene(string sceneName, UnityAction action)
        {
            SerializedScene aScene = scenesDataConfig.GetSerializedScene(sceneName);

            aScene.OnLoad.AddListener(action);
        }
        
        /// <summary>
        /// Unsubscribes an event from the Add scene event
        /// </summary>
        /// <param name="sceneName">Scene name to unload the action</param>
        /// <param name="action">Action to unsubscribe.</param>
        public void UnsubscribeEventToAddScene(string sceneName, UnityAction action)
        {
            SerializedScene aScene = scenesDataConfig.GetSerializedScene(sceneName);

            aScene.OnLoad.RemoveListener(action);
        }
    }
}
