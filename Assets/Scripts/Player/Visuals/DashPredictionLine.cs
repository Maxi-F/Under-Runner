using Player;
using UnityEngine;

public class DashPredictionLine : MonoBehaviour
{
    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private LineRenderer renderer;
    [SerializeField] private float lineLength;

    void Update()
    {
        //renderer.SetPosition(0, transform.position);
        renderer.SetPosition(1, (movementController.CurrentDir.normalized * lineLength));
    }

    public void ToggleVisibility(bool value)
    {
        gameObject.SetActive(value);
    }
}