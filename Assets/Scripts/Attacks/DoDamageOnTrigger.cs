using System.Collections;
using Health;
using UnityEngine;

namespace Attacks
{
    public class DoDamageOnTrigger : MonoBehaviour
    {
        [SerializeField] private int damage = 5;
        [SerializeField] private string entityDamageTaker = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(entityDamageTaker))
            {
                ITakeDamage damageTaker = other.GetComponent<ITakeDamage>();
                damageTaker.TryTakeDamage(damage);
            }
        }
    }
}