using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif

namespace Scenes.ScriptableObjects
{

    [CreateAssetMenu(menuName = "Create ScenesData", fileName = "ScenesData", order = 0)]
    public class ScenesDataConfig : ScriptableObject
    {
        public List<SerializedScene> scenes;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            CheckScenes();
        }

        public SerializedScene GetSerializedScene(string sceneName)
        {
            return scenes.Find(scene => scene.sceneName == sceneName);
        }
        
        /// <summary>
        /// Remakes the List of serializable scenes, depending on build settings.
        /// </summary>
        private void CheckScenes()
        {
            foreach (var editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                string sceneName = new DirectoryInfo(editorBuildSettingsScene.path).Name.Split('.')[0];
                if (editorBuildSettingsScene.enabled)
                {
                    int index = SceneUtility.GetBuildIndexByScenePath(editorBuildSettingsScene.path);

                    if (index < scenes.Count && scenes[index] != null)
                    {
                        scenes[index].index = index;
                    }
                    else
                    {
                        SerializedScene serializedScene = new SerializedScene
                        {
                            index = index,
                            sceneName = sceneName,
                            sceneGuid = editorBuildSettingsScene.guid.ToString()
                        };
                        
                        scenes.Add(serializedScene);
                    }
                    
                }
            }
        }

#endif
    }
}