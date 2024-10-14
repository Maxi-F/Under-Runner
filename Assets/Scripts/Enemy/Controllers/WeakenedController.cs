using System.Collections;
using Enemy;
using Enemy.Shield;
using Events;
using FSM;
using Health;
using UnityEngine;

public class WeakenedController : EnemyController
{
    [Header("Properties")]
    [SerializeField] private HealthPoints healthPoints;
    [SerializeField] private ShieldController shieldController;
    [SerializeField] private bool shieldActive;

    [Header("ShieldProperties")]
    [SerializeField] private float timeToReactivateShield = 4.0f;
    [SerializeField] private float timeToStartReactivatingShield = 2.0f;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onEnemyDeathEvent;
    [SerializeField] private BoolEventChannelSO onEnemyParriedEvent;
    [SerializeField] private IntEventChannelSO onEnemyDamageEvent;

    private void OnEnable()
    {
        healthPoints.SetCanTakeDamage(false);
        shieldController.SetActive(true);
        shieldController.SetActiveMaterial();

        onEnemyDeathEvent?.onEvent.AddListener(HandleDeath);
    }

    private void OnDisable()
    {
        onEnemyDamageEvent?.onEvent.RemoveListener(HandleDeath);
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }

    public void HandleShield(bool isActive)
    {
        if (!isActive && !shieldController.IsActive()) return;

        healthPoints.SetCanTakeDamage(!isActive);

        shieldController.SetActive(isActive);
        
        onEnemyParriedEvent?.RaiseEvent(isActive);
        if (isActive)
        {
            shieldController.ResetShield();
            enemyAgent.ChangeStateToIdle();
        }
        else
        {
            StartCoroutine(ReactivateShield());
        }
    }

    private IEnumerator ReactivateShield()
    {
        yield return new WaitForSeconds(timeToStartReactivatingShield);

        shieldController.SetIsActivating(true);

        yield return new WaitForSeconds(timeToReactivateShield);

        shieldController.SetActiveMaterial();

        HandleShield(true);
    }
}