using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SituationManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI promptDisplay;
    public GameObject buttonContainer; // Drag the object holding your 4 buttons here
    public Button[] choiceButtons;     // Drag the 4 buttons here

    [Header("Activity Content")]
    public List<SituationData> questionList;
    [TextArea(2, 5)]
    public string introMessage = "Guten Tag! Bist du bereit für den Test? Drücke nochmal 'W' um anzufangen!";

    [Header("Rewards")]
    public Animator doorAnimator; 

    private int currentQuestionIndex = -1; // -1 means we are showing the Intro
    private int correctAnswersCount = 0;
    private bool isQuizActive = false;
    public bool isFinished = false;

    // This runs automatically when the UI is turned on via the NPC script
    void OnEnable()
    {
        currentQuestionIndex = -1; 
        correctAnswersCount = 0;
        isQuizActive = false;
        ShowIntro();
    }

    void ShowIntro()
    {
        // 1. Show Herr Schmidt's welcome message
        promptDisplay.text = introMessage;

        // 2. Hide the answer buttons so they don't see them yet
        if (buttonContainer != null) buttonContainer.SetActive(false);
    }

    // This is called by the NpcInteraction script when you press W a second time
    public void StartFirstQuestion()
    {
        if (isQuizActive) return; // Prevent restarting if already in the quiz

        isQuizActive = true;
        currentQuestionIndex = 0;

        // Show the buttons now that the quiz started
        if (buttonContainer != null) buttonContainer.SetActive(true);
        
        ShowQuestion();
    }

    void ShowQuestion()
    {
        SituationData data = questionList[currentQuestionIndex];
        promptDisplay.text = data.questionText;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // Set button text
            TextMeshProUGUI btnText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = data.answers[i];

            // Setup button click
            int index = i; 
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnAnswerClicked(index));
        }
    }

    void OnAnswerClicked(int selectedIndex)
    {
        // Check answer
        if (selectedIndex == questionList[currentQuestionIndex].correctIndex)
        {
            correctAnswersCount++;
        }
        else 
        {
            TriggerSubtleShake();
        }

        // Next question
        currentQuestionIndex++;

        if (currentQuestionIndex < questionList.Count)
        {
            ShowQuestion();
        }
        else
        {
            CalculateFinalScore();
        }
    }

    void CalculateFinalScore()
{
    // 1. Logic States
    isQuizActive = false;
    isFinished = true; // Tells TeacherInteraction.cs that W should now CLOSE the UI

    // 2. UI Cleanup
    if (buttonContainer != null) buttonContainer.SetActive(false);

    // 3. Score Calculation
    float scorePercent = ((float)correctAnswersCount / questionList.Count) * 100f;
    promptDisplay.text = $"Ergebnis: {scorePercent}%";

    // 4. Success or Failure Logic
    if (scorePercent >= 80f)
    {
        promptDisplay.text += "\n\nAusgezeichnet! Die Tür ist offen.";
        
        if (doorAnimator != null) 
        {
            // Play the opening animation
            doorAnimator.SetTrigger("Open");

            // PHYSICS FIX: Turn off the collider so the player can walk through
            Collider2D doorCol = doorAnimator.GetComponent<Collider2D>();
            if (doorCol != null) 
            {
                doorCol.enabled = false; 
                Debug.Log("Door is now open and passable!");
            }
        }
    }
    else
    {
        promptDisplay.text += "\n\nDas reicht nicht. Lern mehr mit Herr Schmidt!";
        // Note: isFinished is still true, so the player can press W to close 
        // the menu and try talking to him again to restart.
    }
}

    void TriggerSubtleShake()
    {
        var source = GameObject.Find("ShakeManager")?.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
        if (source != null) source.GenerateImpulse(0.2f);
    }
}