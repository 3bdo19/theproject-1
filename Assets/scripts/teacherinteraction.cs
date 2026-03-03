using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [Header("UI to Activate")]
    public GameObject situationUI; // Drag your 'SituationActivity' folder here
    public GameObject wPrompt;
    
    private bool playerIsClose = false;

   void Update()
{
    if (situationUI == null) return; // Safety check for that "Null" error

    if (playerIsClose && Input.GetKeyDown(KeyCode.W))
    {
        // If the UI is already open...
        if (situationUI.activeSelf)
        {
            SituationManager manager = situationUI.GetComponentInChildren<SituationManager>();
            
            if (manager != null)
            {
                // If the quiz is done, W now closes the window
                if (manager.isFinished) 
                {
                    situationUI.SetActive(false);
                    // Resume the world music because the conversation is over
                    FindObjectOfType<scripts_manager>().ResumeWorldMusic();
                    Debug.Log("Dialogue Closed.");
                }
                // If the quiz isn't started yet, W starts the questions
                else 
                {
                    manager.StartFirstQuestion();
                }
            }
        }
        else
        {
            // Open the UI (shows the Intro)
            situationUI.SetActive(true);
            if(wPrompt != null) wPrompt.SetActive(false);
        }
    }
}
    void StartDialogue()
    {
        if (situationUI != null)
        {
            // Find the manager and pause the world music
            FindObjectOfType<scripts_manager>().PauseWorldMusic();
            situationUI.SetActive(true);
            Debug.Log("Dialogue Started!");
        }
    }

    // Unity automatically calls this when the player enters the circle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if(wPrompt != null) wPrompt.SetActive(true);
        }
    }

    // Unity calls this when the player leaves the circle
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            if(wPrompt != null) wPrompt.SetActive(false);
        }
    }
}