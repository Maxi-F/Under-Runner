using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject swingPivot;
        [SerializeField] private GameObject laserObject;
        [SerializeField] private SwingConfigSO swingConfigSo;

        private Quaternion _startingRotation;
        private Quaternion _finishingRotation;

        private Sequence _swingSequence;
        private Coroutine swingCoroutine;

        public SwingConfigSO SwingConfigSo
        {
            get { return swingConfigSo; }
            set { swingConfigSo = value; }
        }
        
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
            swingPivot.SetActive(true);

            _startingRotation = Quaternion.Euler(0, swingConfigSo.startingDegreesY, 0);
            _finishingRotation = Quaternion.Euler(0, swingConfigSo.finishingDegreesY, 0);

            laserObject.transform.localScale = swingConfigSo.initialLaserScale;
            laserObject.transform.localPosition = swingConfigSo.initialLaserLocalPosition;

            swingPivot.transform.localRotation = _startingRotation;
            yield break;
        }

        private IEnumerator LaserGrowCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo.growDuration)
            {
                timer = Time.time - startingTime;

                float scaleUpValue = Mathf.Lerp(swingConfigSo.startingSize, swingConfigSo.finishingSize, swingConfigSo.laserGrowCurve.Evaluate(timer / swingConfigSo.growDuration));

                Vector3 scale = new Vector3(scaleUpValue, 1, 1);

                laserObject.transform.localPosition = new Vector3(swingConfigSo.initialLaserLocalPosition.x + scaleUpValue / 2, 0, 0);
                laserObject.transform.localScale = scale;
                yield return null;
            }
        }

        private IEnumerator SwingingCoroutine()
        {
            float timer = 0;
            float startingTime = Time.time;

            while (timer < swingConfigSo.swingDuration)
            {
                timer = Time.time - startingTime;
                float swingValue = swingConfigSo.swingCurve.Evaluate(timer / swingConfigSo.swingDuration);
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