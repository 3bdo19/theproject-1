using UnityEngine;

public class LessonBoardInteract : MonoBehaviour
{
    public GameObject lessonPanel; 
    public GameObject promptText;  
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.W))
        {
            ToggleBoard();
        }
    }

    public void ToggleBoard()
    {
        bool isActive = !lessonPanel.activeSelf;
        lessonPanel.SetActive(isActive);

        if (isActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            promptText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            promptText.SetActive(false);
            lessonPanel.SetActive(false); 
        }
    }
}