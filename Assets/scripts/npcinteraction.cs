using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    // ... [Existing UI and Content Slots] ...
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

    [Header("Voice Settings")] // NEW SECTION
    [Tooltip("Type 'HerrSchmidt' or 'SecondCharacter' to match scripts_manager")]
    public string voiceCharacterName = "HerrSchmidt"; 
    public bool useCatVoiceInstead = false; 

    [Header("Settings")]
    public float typingSpeed = 0.05f; 
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // ... [Existing Variables and Start/Update/Flip logic] ...
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
        if (isPlayerNearby && !dialoguePanel.activeSelf)
        {
            if (interactPrompt != null) interactPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return))
            {
                scripts_manager manager = FindObjectOfType<scripts_manager>();
                if (manager != null) manager.PauseWorldMusic();
                StartDialogue();
                return;
            }
        }
        else if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

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

        UpdateUI(); // Changed to UpdateUI to trigger typing and sound on the first line
    }

    // --- MODIFIED TYPETEXT COROUTINE ---
    IEnumerator TypeText(string german, string arabic)
    {
        isTyping = true;
        germanText.text = "";
        arabicText.text = "";

        scripts_manager manager = FindObjectOfType<scripts_manager>();
        if (manager != null)
        {
            // If the toggle is off, play the specific character voice
            if (!useCatVoiceInstead)
            {
                manager.PlayCharacterVoice(voiceCharacterName);
            }
            else
            {
                manager.PlayRandomCatVoiceOnce();
            }
        }

        char[] letters = german.ToCharArray();

        for (int i = 0; i < letters.Length; i++)
        {
            germanText.text += letters[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        
        // No need to stop cat voice if we used a OneShot character voice, but safe to keep
        if (manager != null) manager.StopCatVoice();
         
        arabicText.text = arabic; 
        isTyping = false;
    }

    // ... [Rest of your existing functions: NextLine, UpdateUI, EndDialogue, etc.] ...
    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            FindObjectOfType<scripts_manager>().StopCatVoice();
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

    void UpdateUI()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(germanLines[index], arabicLines[index]));
    }

    void EndDialogue()
    {
        scripts_manager manager = FindObjectOfType<scripts_manager>();
        if (manager != null) manager.ResumeWorldMusic();
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
}