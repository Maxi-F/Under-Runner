using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onSwingEndEvent;
        [SerializeField] private GameObject swingPivot;
        [SerializeField] private GameObject laserObject;
        [SerializeField] private Vector3 initialLaserLocalPosition;
        [SerializeField] private Vector3 initialLaserScale;
        [SerializeField] private float growDuration = 1.4f;
        [SerializeField] private float swingDuration = 1.4f;

        [Header("Swing Configuration")]
        [SerializeField] private AnimationCurve swingCurve;
        [SerializeField] private float startingDegreesY;
        [SerializeField] private float finishingDegreesY;

        [Header("Laser Grow Configuration")]
        [SerializeField] private AnimationCurve laserGrowCurve;
        [SerializeField] private float startingSize;
        [SerializeField] private float finishingSize;

        private Quaternion _startingRotation;
        private Quaternion _finishingRotation;

        private void OnEnable()
        {
            onSwingEndEvent?.onEvent.AddListener(HandleSwingEndEvent);
        }

        private void OnDisable()
        {
            onSwingEndEvent?.onEvent.RemoveListener(HandleSwingEndEvent);
        }

        [ContextMenu("Swing")]
        public void RunSwing()
        {
            StartCoroutine(SwingSequence());
        }

        public IEnumerator SwingSequence()
        {
            swingPivot.SetActive(true);

            _startingRotation = Quaternion.Euler(0, startingDegreesY, 0);
            _finishingRotation = Quaternion.Euler(0, finishingDegreesY, 0);

            laserObject.transform.localScale = initialLaserScale;
            laserObject.transform.localPosition = initialLaserLocalPosition;

            swingPivot.transform.localRotation = _startingRotation;

            yield return LaserGrowCoroutine();
            yield return SwingingCoroutine();

            swingPivot.SetActive(false);
        }

        private IEnumerator SwingingCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingDuration)
            {
                timer = Time.time - startingTime;
                float swingValue = swingCurve.Evaluate(timer / swingDuration);
                swingPivot.transform.localRotation = Quaternion.Slerp(_startingRotation, _finishingRotation, swingValue);
                yield return null;
            }
        }

        private IEnumerator LaserGrowCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < growDuration)
            {
                timer = Time.time - startingTime;

                float scaleUpValue = Mathf.Lerp(startingSize, finishingSize, laserGrowCurve.Evaluate(timer / growDuration));

                Vector3 scale = new Vector3(scaleUpValue, 1, 1);

                laserObject.transform.localPosition = new Vector3(initialLaserLocalPosition.x + scaleUpValue / 2, 0, 0);
                laserObject.transform.localScale = scale;
                yield return null;
            }
        }

        private void HandleSwingEndEvent()
        {
            gameObject.SetActive(false);
        }
    }
}