using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Minion
{
    [RequireComponent(typeof(MinionAgent))]
    public class MinionStartSequence : MonoBehaviour
    {
        [SerializeField] private float yStartPlusPosition;
        [SerializeField] private float yVelocity;
        
        private MinionAgent _minionAgent;
        private float _finishYPosition;
        private bool _isInFinishPosition;
        
        private void OnEnable()
        {
            _minionAgent = GetComponent<MinionAgent>();
            _minionAgent.enabled = false;
            _isInFinishPosition = false;

            _finishYPosition = transform.position.y;
            float yPosition = _finishYPosition + yStartPlusPosition;
            
            transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
            
            StartCoroutine(GetStartSequence());
        }

        private IEnumerator GetDownCoroutine()
        {
            while (transform.position.y > _finishYPosition)
            {
                transform.position += Vector3.down * (yVelocity * Time.deltaTime);
                yield return null;
            }

            _isInFinishPosition = true;
        }

        private IEnumerator GetDown()
        {
            StartCoroutine(GetDownCoroutine());
            yield return new WaitUntil(() => _isInFinishPosition);
        }
        
        private IEnumerator StartMinion()
        {
            _minionAgent.enabled = true;
            yield return null;
        }
        
        private IEnumerator GetStartSequence()
        {
            Sequence startSequence = new Sequence();
            
            startSequence.AddPreAction(GetDown());
            startSequence.SetAction(StartMinion());

            yield return startSequence.Execute();
        }

    }
}
