using System.Threading;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    private float _speed = 6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * Time.deltaTime * transform.right;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            transform.gameObject.SetActive(false);
        }

        //after hit just disable?
        transform.gameObject.SetActive(false);
    }
}
