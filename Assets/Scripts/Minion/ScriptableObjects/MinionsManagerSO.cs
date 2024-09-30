using UnityEngine;

namespace Minion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Minions/Manager Config")]
    public class MinionsManagerSO : ScriptableObject
    {
        public int maxMinionsAttackingAtSameTime;
        public int maxMinionsAtSameTime;
    }
}
