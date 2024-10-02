using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class DodgeHandler : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onDodgeEvent;

    [Header("Bullet Time Settings")]
    [SerializeField] private float bulletTimeDuration;
    [SerializeField] private AnimationCurve bulletTimeVariationCurve;

    private Coroutine dodgeBulletTimeCoroutine;

    private void OnEnable()
    {
        onDodgeEvent.onEvent.AddListener(HandleDodgeEvent);
    }

    private void OnDisable()
    {
        onDodgeEvent.onEvent.RemoveListener(HandleDodgeEvent);
    }

    private void HandleDodgeEvent()
    {
        if (dodgeBulletTimeCoroutine != null)
            StopCoroutine(dodgeBulletTimeCoroutine);

        dodgeBulletTimeCoroutine = StartCoroutine(DodgeBulletTimeCoroutine());
    }

    private IEnumerator DodgeBulletTimeCoroutine()
    {
        float timer = 0;
        float startTime = Time.time;

        while (timer < bulletTimeDuration)
        {
            timer = Time.time - startTime;
            float timerProgress = Mathf.Lerp(0, 1, timer / bulletTimeDuration);
            Time.timeScale = bulletTimeVariationCurve.Evaluate(timerProgress);
            yield return null;
        }

        Time.timeScale = 1;
    }
}