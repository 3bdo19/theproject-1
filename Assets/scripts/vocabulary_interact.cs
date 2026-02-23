using UnityEngine;
using TMPro;

public class VocabularyInteract : MonoBehaviour
{
    [Header("UI References")]
    public GameObject promptUI;     // Drag "Drücke W" here
    public GameObject wordPanel;    // Drag your White/Yellow Panel here
    public Animator panelAnimator;  // Drag the Panel here (for the pop-up animation)

    private bool isPlayerNearby = false;
    private bool isPanelOpen = false;

    void Start()
    {
        // Ensure everything starts hidden
        if(promptUI) promptUI.SetActive(false);
        if(wordPanel) wordPanel.SetActive(false);
    }

    void Update()
    {
        // If player is near and presses W
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.W))
        {
            TogglePanel();
        }
    }

    void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;

        if (isPanelOpen)
        {
            wordPanel.SetActive(true);
            if (panelAnimator != null) panelAnimator.SetTrigger("Open");
            promptUI.SetActive(false); // Hide prompt while reading
        }
        else
        {
            wordPanel.SetActive(false);
            promptUI.SetActive(true); // Show prompt again
        }
    }

    // Detection Logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (!isPanelOpen) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            isPanelOpen = false;
            wordPanel.SetActive(false);
            promptUI.SetActive(false);
        }
    }
}