using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered is the Player
        if (collision.CompareTag("Player"))
        {
            scripts_manager manager = FindObjectOfType<scripts_manager>();
            
            if (manager != null)
            {
                // Call your existing function that enables the UI
                manager.TriggerGameOver(); 
            }
        }
    }
}