using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scenes.ScriptableObjects
{
    [Serializable]
    public class SceneData
    {
        public string name;
        public int index;
    }
    
    [CreateAssetMenu(menuName = "Scenes/config")]
    public class ScenesSO : ScriptableObject
    {
        public List<SceneData> scenes;
    }
}
