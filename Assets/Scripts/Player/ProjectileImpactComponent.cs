using UnityEngine;

public class ProjectileImpactComponent : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(DestroyImpact), 0.4f);
    }

    private void DestroyImpact()
    {
        Destroy(gameObject);
    }
}
