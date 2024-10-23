using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneryManager sceneryManager;
        private void Awake()
        {
            sceneryManager.InitScenes();
        }
    }
}
