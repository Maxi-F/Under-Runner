using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.FallingBlock
{
    public class FallingBlock : MonoBehaviour
    {
        [SerializeField] private Vector2 initialHeightRange = new Vector2(10.0f, 15.0f);
        [SerializeField] private float heightToDestroy = -1f;
        [SerializeField] private GameObject parentObject;
        
        void Start()
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Random.Range(initialHeightRange.x, initialHeightRange.y);
            transform.position = newPosition;
        }
        
        void Update()
        {
            if (transform.position.y < heightToDestroy)
            {
                Destroy(parentObject);
            }
        }
    }
}
