using System.Collections;
using UnityEngine;

namespace Bullet
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField] private int spawnInSeconds = 2;
        [SerializeField] private GameObject bullet;
    
        private bool _isSpawning = false;
    

        // Update is called once per frame
        void Update()
        {
            if (!_isSpawning)
            {
                StartCoroutine(SpawnBullet());
                _isSpawning = true;
            }
        }

        IEnumerator SpawnBullet()
        {
            yield return new WaitForSeconds(spawnInSeconds);
            GameObject bulletInstance = Instantiate(bullet, transform, true);
            bulletInstance.transform.position = transform.position;
            
            _isSpawning = false;
        }
    }
}
