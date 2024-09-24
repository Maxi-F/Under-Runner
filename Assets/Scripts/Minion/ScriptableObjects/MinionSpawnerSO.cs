using MapBounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minion.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Minions/Spawner Config")]
    public class MinionSpawnerSO : ScriptableObject
    {
        public MapBoundsSO spawnBounds;
        public int minionsToSpawn;
        public float timeBetweenSpawns;
        
        public Vector3 GetSpawnPoint()
        {
            return new Vector3(
                Random.Range(spawnBounds.horizontalBounds.min, spawnBounds.horizontalBounds.max),
                1.0f,
                Random.Range(spawnBounds.depthBounds.min, spawnBounds.depthBounds.max)
            );
        }
    }
}
