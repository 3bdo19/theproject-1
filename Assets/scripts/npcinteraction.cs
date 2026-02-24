using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI GameObjects")]
    public GameObject dialoguePanel;
    public GameObject interactPrompt;

    [Header("UI Content Slots")]
    public Image portraitImageSlot;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI germanText;
    public TextMeshProUGUI arabicText;

    [Header("Dialogue Content")]
    public string charName;
    public Sprite characterCloseUp;
    [TextArea(3, 10)] public string[] germanLines;
    [TextArea(3, 10)] public string[] arabicLines;

    [Header("Settings")]
    public float typingSpeed = 0.05f; // How fast the letters appear
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private int index = 0;
    private bool isPlayerNearby = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        if (interactPrompt != null) interactPrompt.SetActive(false);
        if (portraitImageSlot != null) portraitImageSlot.gameObject.SetActive(false);
    }

    void Update()
    {
        // 1. Logic for starting the dialogue
        if (isPlayerNearby && !dialoguePanel.activeSelf)
        {
            if (interactPrompt != null) interactPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return))
            {
                StartDialogue();
                return;
            }
        }
        
        else if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        // 2. Logic for moving to the next line
        if (dialoguePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(1))
            {
                NextLine();
            }
        }

        if (isPlayerNearby) FlipTowardsPlayer();
    }

    public void StartDialogue()
    {
        index = 0;
        dialoguePanel.SetActive(true);
        nameText.text = charName;

        if (portraitImageSlot != null && characterCloseUp != null)
        {
            portraitImageSlot.sprite = characterCloseUp;
            portraitImageSlot.gameObject.SetActive(true);
        }

        ShowLines();
    }

    public void NextLine()
    {
        if (isTyping)
        {
            // If the player clicks while it's typing, finish the line instantly
            StopCoroutine(typingCoroutine);
            germanText.text = germanLines[index];
            arabicText.text = arabicLines[index];
            isTyping = false;
        }
        else if (index < germanLines.Length - 1)
        {
            index++;
            UpdateUI();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowLines()
    {
        // Text appears instantly without any typewriter effect
        germanText.text = germanLines[index];
        arabicText.text = arabicLines[index];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        if (portraitImageSlot != null) portraitImageSlot.gameObject.SetActive(false);
        index = 0; 
    }

    void FlipTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 currentScale = transform.localScale;
            if (player.transform.position.x > transform.position.x)
                currentScale.x = Mathf.Abs(currentScale.x);
            else
                currentScale.x = -Mathf.Abs(currentScale.x);
            transform.localScale = currentScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            EndDialogue();
        }
    }

    void UpdateUI()
    {
        // If we were already typing, stop it before starting a new line
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(germanLines[index], arabicLines[index]));
    }

    IEnumerator TypeText(string german, string arabic)
    {
        isTyping = true;
        germanText.text = "";
        arabicText.text = "";

        // Type the German text first
        foreach (char letter in german.ToCharArray())
        {
            germanText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Once German is done, show the Arabic
        // You can make this type out too, or just snap it in
        arabicText.text = arabic; 
        
        isTyping = false;
    }

    public void PreviousLine()
    {
    if (index > 0)
    {
        index--;
        ShowLines(); // Updates the text to the previous index instantly
    }
    }
}