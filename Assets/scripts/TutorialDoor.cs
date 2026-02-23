using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    public GameObject promptIcon;   // Your "Drücke ⬆️" UI
    public GameObject doorObject;   // The Door with the Animator component
    
    private bool isPlayerNearby = false;
    private bool isDoorOpen = false; 

    void Update()
    {
        // Check if player is near, presses W or Up, and the door is closed
        if (isPlayerNearby && !isDoorOpen && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            OpenTheDoor();
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
}