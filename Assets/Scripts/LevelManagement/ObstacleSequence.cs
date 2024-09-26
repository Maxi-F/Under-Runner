using LevelManagement;
using ObstacleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ObstacleSequence : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private ObstaclesSpawner obstaclesSpawner;

    [Header("UI")]
    [SerializeField] private Slider progressBar;

    private bool _isObstacleSystemDisabled;
    private LevelLoopSO _levelConfig;
    private IEnumerator _postAction;

    public Sequence GetObstacleSequence()
    {
        Sequence obstacleSequence = new Sequence();

        obstacleSequence.AddPreAction(ObstacleSequencePreActions());
        obstacleSequence.SetAction(ObstaclesAction());
        obstacleSequence.AddPostAction(ObstaclesPostActions());
        obstacleSequence.AddPostAction(_postAction);

        return obstacleSequence;
    }

    public void SetLevelConfig(LevelLoopSO levelConfig)
    {
        _levelConfig = levelConfig;
    }

    public void SetPostAction(IEnumerator postAction)
    {
        _postAction = postAction;
    }

    private IEnumerator ObstacleSequencePreActions()
    {
        obstaclesSpawner.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
        _isObstacleSystemDisabled = false;

        yield return null;
    }

    private IEnumerator ObstaclesAction()
    {
        float timer = 0;
        float obstaclesDuration = _levelConfig.obstacleData.obstaclesDuration;
        float obstacleCooldown = _levelConfig.obstacleData.obstacleCooldown;
        float startTime = Time.time;

        obstaclesSpawner.StartWithCooldown(obstacleCooldown);

        while (timer < obstaclesDuration)
        {
            timer = Time.time - startTime;
            progressBar.value = Mathf.Lerp(0, progressBar.maxValue, timer / obstaclesDuration);
            yield return null;
        }

        progressBar.gameObject.SetActive(false);
        obstaclesSpawner.Disable();
    }

    private IEnumerator ObstaclesPostActions()
    {
        yield return new WaitUntil(() => _isObstacleSystemDisabled);
    }

}
