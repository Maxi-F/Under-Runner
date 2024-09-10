using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class DashPredictionLine : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private LineRenderer renderer;
    [SerializeField] private float lineLength;

    void Update()
    {
        //renderer.SetPosition(0, transform.position);
        renderer.SetPosition(1, (movement.CurrentDir.normalized * lineLength));
    }

    public void ToggleVisibility(bool value)
    {
        gameObject.SetActive(value);
    }
}