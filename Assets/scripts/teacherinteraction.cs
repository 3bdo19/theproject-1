using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    [Header("UI to Activate")]
    public GameObject situationUI; 
    public GameObject wPrompt;
    
    private bool playerIsClose = false;

   void Update()
{
    if (situationUI == null) return;

    if (playerIsClose && Input.GetKeyDown(KeyCode.W))
    {
        
        if (situationUI.activeSelf)
        {
            SituationManager manager = situationUI.GetComponentInChildren<SituationManager>();
            
            if (manager != null)
            {
                
                if (manager.isFinished) 
                {
                    situationUI.SetActive(false);
                    
                    FindObjectOfType<scripts_manager>().ResumeWorldMusic();
                    Debug.Log("Dialogue Closed.");
                }
               
                else 
                {
                    manager.StartFirstQuestion();
                }
            }
        }
        else
        {
            
            situationUI.SetActive(true);
            if(wPrompt != null) wPrompt.SetActive(false);
        }
    }
}
    void StartDialogue()
    {
        if (situationUI != null)
        {
            
            FindObjectOfType<scripts_manager>().PauseWorldMusic();
            situationUI.SetActive(true);
            Debug.Log("Dialogue Started!");
        }
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if(wPrompt != null) wPrompt.SetActive(true);
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            if(wPrompt != null) wPrompt.SetActive(false);
        }
    }
}