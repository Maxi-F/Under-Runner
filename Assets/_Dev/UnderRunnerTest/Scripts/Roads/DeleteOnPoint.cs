using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Roads
{
    public class DeleteOnPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 pointToDeleteOn;
        [SerializeField] private GameObject roadObject;
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.x < pointToDeleteOn.x)
            {
                Destroy(roadObject);
            }
        }
    }
}
