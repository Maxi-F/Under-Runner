using System.Collections;
using Events;
using Health;
using Input;
using MapBounds;
using Player.Controllers;
using UnityEngine;
#if UNITY_EDITOR
#endif

#if UNITY_EDITOR
#endif

namespace Player
{
    public class PlayerDashController : PlayerController
    {
        [Header("Input")]
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("MapBounds")]
        [SerializeField] private MapBoundsSO boundsConfig;

        [Header("Dash Configuration")]
        [SerializeField] private float dashSpeed;

        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private AnimationCurve speedCurve;

        [Header("Bullet Time Dash")]
        [SerializeField] private DashPredictionLine dashPredictionLine;

        [SerializeField] private AnimationCurve bulletTimeVariationCurve;
        [SerializeField] private float bulletTimeDuration;

        [Header("Events")]
        [SerializeField] private FloatEventChannelSO onDashRechargeEvent;
        [SerializeField] private VoidEventChannelSO onDashRechargedEvent;
        [SerializeField] private VoidEventChannelSO onDashUsedEvent;

        private PlayerMovementController _movementController;
        private HealthPoints _healthPoints;

        private bool _canDash = true;
        private Coroutine _dashCoroutine = null;
        private Coroutine _bulletTimeCoroutine = null;

        private Vector3 _dashDir;
        private float _currentDashSpeed;

        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
            _healthPoints ??= GetComponent<HealthPoints>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            inputHandler.onPlayerDash.AddListener(HandleDash);

            inputHandler.onPlayerDashStarted.AddListener(HandleBulletTimeDashStart);
            inputHandler.onPlayerDashFinished.AddListener(HandleBulletTimeDashFinish);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerDash.RemoveListener(HandleDash);
        }

        private void HandleDash()
        {
            if (!_canDash)
                return;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            _healthPoints.SetIsInvincible(true);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        public void HandleBulletTimeDashStart()
        {
            if (!_canDash)
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);

            _bulletTimeCoroutine = StartCoroutine(BulletTimeCoroutine());
        }

        public void HandleBulletTimeDashFinish()
        {
            if (!_canDash || Time.timeScale == 1)
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);
            Time.timeScale = 1f;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            dashPredictionLine.ToggleVisibility(false);
            _healthPoints.SetIsInvincible(true);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            float timer = 0;
            _canDash = false;

            _dashDir = _movementController.CurrentDir;
            _movementController.ToggleMoveability(false);
            onDashUsedEvent.RaiseEvent();
            while (timer < dashDuration)
            {
                float dashTime = Mathf.Lerp(0, 1, timer / dashDuration);

                _currentDashSpeed = dashSpeed * speedCurve.Evaluate(dashTime);

                Vector3 newPosition = transform.position + (_dashDir * (_currentDashSpeed * Time.deltaTime));
                transform.position = boundsConfig.ClampPosition(newPosition, playerCollider.bounds.size);

                timer = Time.time - startTime;
                yield return null;
            }

            _movementController.ToggleMoveability(true);
            _healthPoints.SetIsInvincible(false);
            yield return CoolDownCoroutine();
            _canDash = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            float timeInCooldown = 0f;
            while (timeInCooldown <= dashCoolDown)
            {
                onDashRechargeEvent.RaiseEvent(timeInCooldown);
                yield return null;
                timeInCooldown += Time.deltaTime;
            }

            onDashRechargedEvent.RaiseEvent();
        }

        private IEnumerator BulletTimeCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            dashPredictionLine.ToggleVisibility(true);

            while (timer < bulletTimeDuration)
            {
                timer = Time.time - startTime;
                float timerProgress = Mathf.Lerp(0, 1, timer / bulletTimeDuration);
                Time.timeScale = bulletTimeVariationCurve.Evaluate(timerProgress);
                yield return null;
            }

            dashPredictionLine.ToggleVisibility(false);
            Time.timeScale = 1;
        }
    }
}