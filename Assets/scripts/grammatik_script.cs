using UnityEngine;

public class TutorialIconInteraction : MonoBehaviour
{
    [Header("UI Reference")]

    public GameObject doorObject;   // The Door with the Animator component
    public GameObject promptIcon; 
    public GameObject photoPanel; 

    private bool isPlayerNearby = false;
    private bool isDoorOpen = false; 

     void Update()
    {
        // Check if player is near, presses W or Up, and the door is closed
        if (isPlayerNearby  && !isDoorOpen && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            OpenPhoto();
            OpenTheDoor();
        }
    }

    public void OpenPhoto()
    {
        if (photoPanel != null)
        {
            photoPanel.SetActive(true);
            
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f; 
        }
    }

    public void ClosePhoto()
    {
        photoPanel.SetActive(false);
        
        // Resume game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only show the prompt if the player enters and door is still shut
        if (other.CompareTag("Player") && !isDoorOpen)
        {
            isPlayerNearby = true;
            if (promptIcon != null) promptIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (promptIcon != null) promptIcon.SetActive(false);
        }
    }

    void OpenTheDoor()
    {
        isDoorOpen = true;
        
        // 1. Hide the "Drücke" prompt immediately
        if (promptIcon != null)
        {
            promptIcon.SetActive(false);
        }
        
        // 2. Play the Animation
        // This line fixes the 'anim' error you saw in the console!
        Animator anim = doorObject.GetComponent<Animator>();
        
        if (anim != null)
        {
            // Make sure the name in quotes matches your Animator parameter exactly
            anim.SetTrigger("Open"); 
        }

        // 3. Disable the Door's Collider so the player can walk through
        BoxCollider2D doorCollider = doorObject.GetComponent<BoxCollider2D>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        Debug.Log("Success! Door is opening.");
    }
}