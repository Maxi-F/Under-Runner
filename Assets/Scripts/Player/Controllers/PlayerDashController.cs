using System.Collections;
using Events;
using Health;
using Input;
using Managers;
using MapBounds;
using Player.Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerDashController : PlayerController
    {
        [SerializeField] private PauseSO pauseData;
        
        [Header("Input")]
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("MapBounds")]
        [SerializeField] private MapBoundsSO boundsConfig;

        [Header("Dash Configuration")]
        [SerializeField] private float dashSpeed;

        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private float phantomDuration;
        [SerializeField] private AnimationCurve speedCurve;

        [Header("Bullet Time Dash")]
        [SerializeField] private DashPredictionLine dashPredictionLine;

        [SerializeField] private AnimationCurve bulletTimeVariationCurve;
        [SerializeField] private float bulletTimeDuration;

        [Header("Events")]
        [SerializeField] private FloatEventChannelSO onDashRechargeEvent;
        [SerializeField] private VoidEventChannelSO onDashRechargedEvent;
        [SerializeField] private VoidEventChannelSO onDashUsedEvent;
        [SerializeField] private Vector3EventChannelSO onDashMovementEvent;

        private PlayerMovementController _movementController;
        private HealthPoints _healthPoints;

        private bool _canDash = true;
        private Coroutine _dashCoroutine = null;
        private Coroutine _bulletTimeCoroutine = null;

        private Vector3 _dashDir;
        private float _currentDashSpeed;

        private Bounds _playerColliderBounds;

        private void Awake()
        {
            _movementController = GetComponent<PlayerMovementController>();
            _healthPoints ??= GetComponent<HealthPoints>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _playerColliderBounds = playerCollider.bounds;
            inputHandler.onPlayerDashStarted.AddListener(HandleDash);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerDashStarted.RemoveListener(HandleDash);
        }

        private bool CanDash()
        {
            if (_movementController.CurrentDir == Vector3.zero)
                return false;

            return _canDash;
        }

        private void HandleDash()
        {
            if (!CanDash())
                return;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            _healthPoints.SetIsInvincible(true);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }


        public void HandleBulletTimeDashStart()
        {
            if (!CanDash())
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);

            _bulletTimeCoroutine = StartCoroutine(BulletTimeCoroutine());
        }

        public void HandleBulletTimeDashFinish()
        {
            if (!CanDash() || Time.timeScale == 1)
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);
            Time.timeScale = 1f;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            dashPredictionLine.ToggleVisibility(false);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        public void HandlePhantomCoroutine()
        {
            StartCoroutine(PhantomCoroutine());
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            float timer = 0;
            _canDash = false;

            _dashDir = _movementController.CurrentDir;
            playerAgent.ChangeStateToDash();
            onDashUsedEvent.RaiseEvent();
            while (timer < dashDuration)
            {
                float dashTime = Mathf.Lerp(0, 1, timer / dashDuration);

                _currentDashSpeed = dashSpeed * speedCurve.Evaluate(dashTime);

                Vector3 dashMovement = _dashDir * (_currentDashSpeed * Time.deltaTime);
                Vector3 previousPosition = transform.position;
                Vector3 newPosition = transform.position + dashMovement;

                transform.position = boundsConfig.ClampPosition(newPosition, _playerColliderBounds.size);
                onDashMovementEvent?.RaiseEvent(transform.position - previousPosition);

                timer = Time.time - startTime;
                yield return null;
            }

            if (_movementController.CurrentDir == Vector3.zero)
                playerAgent.ChangeStateToIdle();
            else
                playerAgent.ChangeStateToMove();

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
                yield return new WaitWhile(() => pauseData.isPaused);
                timer = Time.time - startTime;
                float timerProgress = Mathf.Lerp(0, 1, timer / bulletTimeDuration);
                if(!pauseData.isPaused)
                    Time.timeScale = bulletTimeVariationCurve.Evaluate(timerProgress);
                yield return null;
            }

            dashPredictionLine.ToggleVisibility(false);
            Time.timeScale = 1;
        }

        private IEnumerator PhantomCoroutine()
        {
            _healthPoints.SetCanTakeDamage(false);
            yield return new WaitForSeconds(phantomDuration);
            _healthPoints.SetCanTakeDamage(true);
        }

        public void ResetDash()
        {
            _canDash = true;
            onDashRechargedEvent?.RaiseEvent();
        }
    }
}