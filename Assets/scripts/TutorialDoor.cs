using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    public GameObject promptIcon;   
    public GameObject doorObject;   
    private bool isPlayerNearby = false;
    private bool isDoorOpen = false; 

    void Update()
    {
        
        if (isPlayerNearby && !isDoorOpen && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            OpenTheDoor();
        }
    }

    void OpenTheDoor()
    {
        isDoorOpen = true;
        
        
        if (promptIcon != null)
        {
            promptIcon.SetActive(false);
        }
        
        
        Animator anim = doorObject.GetComponent<Animator>();
        
        if (anim != null)
        {
            anim.SetTrigger("Open"); 
        }

        BoxCollider2D doorCollider = doorObject.GetComponent<BoxCollider2D>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        Debug.Log("Success! Door is opening.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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