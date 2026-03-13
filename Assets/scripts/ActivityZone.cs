using UnityEngine;

public class ActivityZone : MonoBehaviour
{
    [Header("References")]
    public GameObject activityCanvas; 
    public WordDestroyer gameManager; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
           
            scripts_manager sManager = FindObjectOfType<scripts_manager>();
            if (sManager != null)
            {
                sManager.StartWordActivity();
            }

            
            if (activityCanvas != null) activityCanvas.SetActive(true);
            if (gameManager != null) gameManager.enabled = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scripts_manager sManager = FindObjectOfType<scripts_manager>();
            if (sManager != null)
            {
                sManager.StopWordActivity();
            }

            if (activityCanvas != null) activityCanvas.SetActive(false);
            if (gameManager != null) gameManager.enabled = false; 
        }
    }
}