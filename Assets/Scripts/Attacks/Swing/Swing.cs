using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject swingPivot;
        [SerializeField] private GameObject laserObject;
        [SerializeField] private List<SwingConfigSO> swingConfigSo = new List<SwingConfigSO>();

        private Quaternion _startingRotation;
        private Quaternion _finishingRotation;

        private Sequence _swingSequence;
        private Coroutine swingCoroutine;

        private int randomIndex;

        [ContextMenu("Swing")]
        private void TestRunSwing()
        {
            SetSequence();
            if (swingCoroutine != null)
                StopCoroutine(swingCoroutine);

            swingCoroutine = StartCoroutine(_swingSequence.Execute());
        }

        public IEnumerator RunSwing()
        {
            SetSequence();
            if (swingCoroutine != null)
                StopCoroutine(swingCoroutine);

            yield return _swingSequence.Execute();
        }

        private void SetSequence()
        {
            _swingSequence = new Sequence();
            _swingSequence.AddPreAction(SwingSetup());
            _swingSequence.AddPreAction(LaserGrowCoroutine());
            _swingSequence.SetAction(SwingingCoroutine());
            _swingSequence.AddPostAction(SwingEnd());
        }

        private IEnumerator SwingSetup()
        {
            randomIndex = Random.Range(0, swingConfigSo.Count);
            swingPivot.SetActive(true);

            _startingRotation = Quaternion.Euler(0, swingConfigSo[randomIndex].startingDegreesY, 0);
            _finishingRotation = Quaternion.Euler(0, swingConfigSo[randomIndex].finishingDegreesY, 0);

            laserObject.transform.localScale = swingConfigSo[randomIndex].initialLaserScale;
            laserObject.transform.localPosition = swingConfigSo[randomIndex].initialLaserLocalPosition;

            swingPivot.transform.localRotation = _startingRotation;
            yield break;
        }

        private IEnumerator LaserGrowCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo[randomIndex].growDuration)
            {
                timer = Time.time - startingTime;

                float scaleUpValue = Mathf.Lerp(swingConfigSo[randomIndex].startingSize, swingConfigSo[randomIndex].finishingSize, swingConfigSo[randomIndex].laserGrowCurve.Evaluate(timer / swingConfigSo[randomIndex].growDuration));

                Vector3 scale = new Vector3(scaleUpValue, 1, 1);

                laserObject.transform.localPosition = new Vector3(swingConfigSo[randomIndex].initialLaserLocalPosition.x + scaleUpValue / 2, 0, 0);
                laserObject.transform.localScale = scale;
                yield return null;
            }
        }

        private IEnumerator SwingingCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo[randomIndex].swingDuration)
            {
                timer = Time.time - startingTime;
                float swingValue = swingConfigSo[randomIndex].swingCurve.Evaluate(timer / swingConfigSo[randomIndex].swingDuration);
                swingPivot.transform.localRotation = Quaternion.Slerp(_startingRotation, _finishingRotation, swingValue);
                yield return null;
            }
        }

        private IEnumerator SwingEnd()
        {
            swingPivot.SetActive(false);
            yield break;
        }
    }
}