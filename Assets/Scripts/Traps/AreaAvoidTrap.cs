using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.SharedLogic;
using UnityEngine;

namespace Assets.Scripts.Traps
{
    public class AreaAvoidTrap : MonoBehaviour
    {
        [SerializeField] private float _damage = 2.0f;
        [SerializeField] private float _delayBetweenHits = 1.0f;
        Dictionary<HealthComponent, Coroutine> _entitiesInTrap = new Dictionary<HealthComponent, Coroutine>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                Debug.Log("ENTERED");
                _entitiesInTrap.Add(healthComponent, StartCoroutine(TrapDamageCoroutine(healthComponent)));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
            {
                Debug.Log("EXITED");
                StopCoroutine(_entitiesInTrap[healthComponent]);
                _entitiesInTrap.Remove(healthComponent);
            }
        }

        IEnumerator TrapDamageCoroutine(HealthComponent healthComp)
        {
            healthComp.GetHit(_damage);
            Debug.Log("DMG");
            while (true)
            {
                yield return new WaitForSeconds(_delayBetweenHits);
                healthComp.GetHit(_damage);
                Debug.Log("DMG");
            }
        }
    }
}
