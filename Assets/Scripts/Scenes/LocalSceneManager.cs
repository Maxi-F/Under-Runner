
using System;
using Events;
using Scenes.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class LocalSceneManager : MonoBehaviour
    {
        [SerializeField] private ScenesSO scenesConfig;
        [SerializeField] private StringEventChannelSo onChangeSceneEvent;
        public static LocalSceneManager Instance;
        
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            } else
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            onChangeSceneEvent?.onStringEvent.AddListener(LoadScene);
        }

        private void OnDisable()
        {
            onChangeSceneEvent?.onStringEvent.RemoveListener(LoadScene);
        }

        public void LoadScene(string sceneName)
        {
            SceneData scene = scenesConfig.scenes.Find(scene => scene.name == sceneName);

            if (scene == null)
            {
                Debug.LogError($"{sceneName} not found.");
                return;
            }

            SceneManager.LoadScene(scene.index);
        }
    }
}
