using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelToBossComponent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("BossFightScene");   
        }
    }
}
