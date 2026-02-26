using UnityEngine;

public class ActivityZone : MonoBehaviour
{
    public GameObject activityCanvas; // Drag WordDestroyer_Canvas here
    public WordDestroyer gameManager; // Drag _WordActivityManager here

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering is the Player
        if (other.CompareTag("Player")) 
        {
            activityCanvas.SetActive(true);
            gameManager.enabled = true; // Starts the spawning timer
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            activityCanvas.SetActive(false);
            gameManager.enabled = false; // Stops spawning if player runs away
        }
    }
}