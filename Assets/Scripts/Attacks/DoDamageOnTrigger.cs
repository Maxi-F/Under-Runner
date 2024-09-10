using System.Collections;
using Health;
using UnityEngine;

namespace Attacks
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
