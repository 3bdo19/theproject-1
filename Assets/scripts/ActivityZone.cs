using UnityEngine;

public class ActivityZone : MonoBehaviour
{
    [Header("References")]
    public GameObject activityCanvas; // Drag WordDestroyer_Canvas here
    public WordDestroyer gameManager; // This matches the name causing the error

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            // 1. Tell the scripts_manager to handle music and scoring
            scripts_manager sManager = FindObjectOfType<scripts_manager>();
            if (sManager != null)
            {
                sManager.StartWordActivity();
            }

            // 2. Start the game visuals and logic
            if (activityCanvas != null) activityCanvas.SetActive(true);
            if (gameManager != null) gameManager.enabled = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Tell the scripts_manager to reset everything
            scripts_manager sManager = FindObjectOfType<scripts_manager>();
            if (sManager != null)
            {
                sManager.StopWordActivity();
            }

            // 2. Stop the game visuals and logic
            if (activityCanvas != null) activityCanvas.SetActive(false);
            if (gameManager != null) gameManager.enabled = false; 
        }
    }
}