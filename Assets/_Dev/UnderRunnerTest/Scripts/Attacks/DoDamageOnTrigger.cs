using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks
{
    public class DoDamageOnTrigger : MonoBehaviour
    {
        [SerializeField] private int damage = 5;
        [SerializeField] private bool isAvoidable = false;
        [SerializeField] private string entityDamageTaker = "Player";
        
        private IEnumerator OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(entityDamageTaker))
            {
                Debug.Log($"Collided with: {other.gameObject.name}");
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();

                yield return null;
                if(isAvoidable)
                    damageTaker.TryTakeAvoidableDamage(damage);
                else 
                    damageTaker.TakeDamage(damage);
            }
        }
    }
}
