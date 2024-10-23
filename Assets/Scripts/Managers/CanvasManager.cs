using System;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [Header("events")] 
        [SerializeField] private BoolEventChannelSO onHandleCanvas;

        private void Awake()
        {
            onHandleCanvas?.onTypedEvent.AddListener(HandleCanvas);
            canvas.SetActive(false);
        }

        private void HandleCanvas(bool value)
        {
            canvas.SetActive(value);
        }
    }
}
