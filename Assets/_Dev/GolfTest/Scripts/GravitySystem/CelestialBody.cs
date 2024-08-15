using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Dev.GolfTest.Scripts.Events;

namespace _Dev.GolfTest.Scripts.GravitySystem
{
    public class CelestialBody : MonoBehaviour
    {
        [Header("Celestial Body Properties")] [SerializeField]
        private float size;

        [Tooltip("Body's Mass In tons")] [SerializeField]
        private float mass;

        [SerializeField] private float atmosphereDrag;
        [SerializeField] private VoidEventChannelSO ballThrown;

        [SerializeField] private float atmosphereShutDownDuration;
        private Coroutine _atmosphereShutDownCoroutine;
        private GameObject _atmosphereGameObject;

        public float Size
        {
            get { return size; }
        }

        public float Mass
        {
            get { return mass; }
        }

        public float AtmosphereDrag
        {
            get { return atmosphereDrag; }
        }

        private void OnValidate()
        {
            transform.localScale = new Vector3(size, size, size);
        }

        private void OnEnable()
        {
            ballThrown.onEvent.AddListener(HandleBallThrown);
            _atmosphereGameObject = transform.Find("Atmosphere").gameObject;
        }

        private void OnDisable()
        {
            ballThrown.onEvent.RemoveListener(HandleBallThrown);
        }

        private void HandleBallThrown()
        {
            if (_atmosphereShutDownCoroutine != null)
                StopCoroutine(_atmosphereShutDownCoroutine);

            _atmosphereShutDownCoroutine = StartCoroutine(TurnOffAtmosphereCoroutine());
        }

        private IEnumerator TurnOffAtmosphereCoroutine()
        {
            _atmosphereGameObject.SetActive(false);
            yield return new WaitForSeconds(atmosphereShutDownDuration);
            _atmosphereGameObject.SetActive(true);
        }
    }
}