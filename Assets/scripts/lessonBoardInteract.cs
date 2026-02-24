using UnityEngine;

public class LessonBoardInteract : MonoBehaviour
{
    public GameObject lessonPanel; // Drag your Scroll View Panel here
    public GameObject promptText;  // The "Press W" UI
    private bool isPlayerNearby = false;

    void Update()
    {
        // Check if player is near and presses W
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.W))
        {
            ToggleBoard();
        }
    }

    public void ToggleBoard()
    {
        bool isActive = !lessonPanel.activeSelf;
        lessonPanel.SetActive(isActive);

        // Handle Mouse Cursor
        if (isActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // Optional: Disable player movement script here
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
            lessonPanel.SetActive(false); // Close automatically if they walk away
        }
    }
}