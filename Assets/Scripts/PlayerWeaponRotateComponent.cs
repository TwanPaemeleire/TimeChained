using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponRotateComponent : MonoBehaviour
{
    private float distanceFromPlayer = 0.6f;
    private ShootComponent _shootComponent;

    private Vector2[] snapDirections = new Vector2[]
    {
        new Vector2(1, 0),    
        new Vector2(1, 1),    
        new Vector2(0, 1),    
        new Vector2(-1, 1),   
        new Vector2(-1, 0),   
        new Vector2(1, -1),   
        new Vector2(-1, -1) 
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shootComponent = GetComponent<ShootComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //mousePos.z = 0f;

        //Vector3 direction = (mousePos - transform.parent.position).normalized;
        //transform.localPosition = direction * distanceFromPlayer;

        //_shootComponent.SetDirection(direction);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        Vector3 dirToMouse = (mouseWorldPos - transform.parent.position).normalized;

        Vector2 bestDir = snapDirections[0];
        float bestDot = Vector2.Dot(dirToMouse, snapDirections[0].normalized);

        foreach (Vector2 snapDir in snapDirections)
        {
            float dot = Vector2.Dot(dirToMouse, snapDir.normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestDir = snapDir;
            }
        }

        Vector3 snappedDirection = bestDir.normalized;
        transform.localPosition = snappedDirection * distanceFromPlayer;

        _shootComponent.SetDirection(snappedDirection);
    }
}
