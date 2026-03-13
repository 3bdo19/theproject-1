using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            scripts_manager manager = FindObjectOfType<scripts_manager>();
            
            if (manager != null)
            {
                manager.TriggerGameOver(); 
            }
        }
    }
}