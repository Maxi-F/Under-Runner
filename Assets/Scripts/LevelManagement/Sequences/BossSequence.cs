using System.Collections;
using Attacks.FallingAttack;
using UnityEngine;
using Utils;

namespace LevelManagement.Sequences
{
    public class BossSequence : MonoBehaviour
    {
        [SerializeField] private FallingBlockSpawner fallingBlockSpawner;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject boss;

        public void SetupSequence(BossData bossData)
        {
            boss.SetActive(false);
            fallingBlockSpawner.SetFallingAttackData(bossData.fallingAttackData);
        }
        
        public IEnumerator StartBossBattle()
        {
            Sequence sequence = new Sequence();

            sequence.SetAction(BossBattleAction());

            return sequence.Execute();
        }
        
        private IEnumerator BossBattleAction()
        {
            yield return null;
            boss.SetActive(true);
        }

        public void ClearSequence()
        {
            boss.SetActive(false);
        }
    }
}
