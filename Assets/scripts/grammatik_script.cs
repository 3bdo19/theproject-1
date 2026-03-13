using UnityEngine;

public class TutorialIconInteraction : MonoBehaviour
{
    [Header("UI Reference")]

    public GameObject doorObject;   
    public GameObject promptIcon; 
    public GameObject photoPanel; 

    private bool isPlayerNearby = false;
    private bool isDoorOpen = false; 

     void Update()
    {
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
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
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
}