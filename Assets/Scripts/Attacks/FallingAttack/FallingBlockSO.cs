using UnityEngine;

namespace Attacks.FallingAttack
{
    [CreateAssetMenu(menuName = "Falling Block Config", fileName = "FallingBlockConfig", order = 0)]
    public class FallingBlockSO : ScriptableObject
    {
        public Vector3 initPosition;
        public GameObject fallingBlockPrefab;
    }
}
