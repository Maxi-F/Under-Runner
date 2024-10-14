using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(menuName = "Pause/Data")]
    public class PauseSO : ScriptableObject
    {
        public bool isPaused;

        public void SetIsPaused(bool value)
        {
            isPaused = value;
        }
    }
}
