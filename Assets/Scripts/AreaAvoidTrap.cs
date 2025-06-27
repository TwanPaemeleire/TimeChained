using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AreaAvoidTrap : MonoBehaviour
{
    [SerializeField] private float _damage = 2.0f;
    [SerializeField] private float _delayBetweenHits = 1.0f;
    [SerializeField] private Sprite _cyberpunkTexture;
    [SerializeField] private Sprite _medievalTexture;
    private SpriteRenderer _spriteComponent;
    Dictionary<HealthComponent, Coroutine> _entitiesInTrap = new Dictionary<HealthComponent, Coroutine>();

    private void Start()
    {
        _spriteComponent = GetComponentInChildren<SpriteRenderer>();
        _spriteComponent.sprite = _cyberpunkTexture;
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
        {
            Debug.Log("ENTERED");
            _entitiesInTrap.Add(healthComponent, StartCoroutine(TrapDamageCoroutine(healthComponent)));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
        {
            Debug.Log("EXITED");
            StopCoroutine(_entitiesInTrap[healthComponent]);
            _entitiesInTrap.Remove(healthComponent);
        }
    }

    void OnWorldSwap()
    {
        if(WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            _spriteComponent.sprite = _cyberpunkTexture;
        }
        else
        {
            _spriteComponent.sprite = _medievalTexture;
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
