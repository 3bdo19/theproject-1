using UnityEngine;
using TMPro;

public class VocabularyInteract : MonoBehaviour
{
    [Header("UI References")]
    public GameObject promptUI;     
    public GameObject wordPanel;    
    public Animator panelAnimator;  

    private bool isPlayerNearby = false;
    private bool isPanelOpen = false;

    void Start()
    {
        if(promptUI) promptUI.SetActive(false);
        if(wordPanel) wordPanel.SetActive(false);
    }

    void Update()
    {
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
            promptUI.SetActive(false); 
        }
        else
        {
            wordPanel.SetActive(false);
            promptUI.SetActive(true); 
        }
    }

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