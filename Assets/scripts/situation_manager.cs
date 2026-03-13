using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class SituationManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI promptDisplay;
    public GameObject buttonContainer; 
    public Button[] choiceButtons;     

    [Header("Activity Content")]
    public List<SituationData> questionList;
    [TextArea(2, 5)]
    public string introMessage = "Guten Tag! Bist du bereit für den Test? Drücke nochmal 'W' um anzufangen!";

    [Header("NPC Settings")]
    [Tooltip("Type 'HerrSchmidt' or 'SecondCharacter' to match the scripts_manager clips")]
    public string voiceCharacterName = "HerrSchmidt"; 
    [Header("Rewards")]
    public Animator doorAnimator; 

    private int currentQuestionIndex = -1; 
    private int correctAnswersCount = 0;
    private bool isQuizActive = false;
    public bool isFinished = false;

    void OnEnable()
    {
        currentQuestionIndex = -1; 
        correctAnswersCount = 0;
        isQuizActive = false;
        isFinished = false; 
        ShowIntro();
    }

    void ShowIntro()
    {
        promptDisplay.text = introMessage;
        if (buttonContainer != null) buttonContainer.SetActive(false);
    }

    public void StartFirstQuestion()
    {
        if (isQuizActive) return; 

        isQuizActive = true;
        currentQuestionIndex = 0;

        if (buttonContainer != null) buttonContainer.SetActive(true);
        
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= questionList.Count)
        {
            Debug.LogWarning("ShowQuestion called with invalid index: " + currentQuestionIndex);
            return;
        }

        SituationData data = questionList[currentQuestionIndex];
        promptDisplay.text = data.questionText;

        scripts_manager manager = Object.FindAnyObjectByType<scripts_manager>();
        if (manager != null)
        {
            manager.PlayCharacterVoice(voiceCharacterName); 
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            TextMeshProUGUI btnText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            
            if (btnText != null && i < data.answers.Length) 
            {
                btnText.text = data.answers[i];
            }

            int index = i; 
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => OnAnswerClicked(index));
        }
    }

   void OnAnswerClicked(int selectedIndex)
    {
        if (selectedIndex == questionList[currentQuestionIndex].correctIndex)
        {
            correctAnswersCount++;
        }
        else 
        {
            TriggerSubtleShake();
        }

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
        isQuizActive = false;
        isFinished = true; 

        if (buttonContainer != null) buttonContainer.SetActive(false);

        float scorePercent = ((float)correctAnswersCount / questionList.Count) * 100f;
        
        if (scorePercent >= 80f)
        {
            promptDisplay.text = $"Ergebnis: {scorePercent}%";
            promptDisplay.text += "\n\nAusgezeichnet! Die Tür ist offen.";
            
            if (doorAnimator != null) 
            {
                doorAnimator.SetTrigger("Open");

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
            Debug.Log("Score below 80%. Triggering Game Over.");
            
            scripts_manager manager = Object.FindAnyObjectByType<scripts_manager>();
            
            if (manager != null)
            {
                manager.TriggerGameOver();
                this.gameObject.SetActive(false); 
            }
            else
            {
                Debug.LogError("Could not find scripts_manager in the scene!");
            }
        }
    }

    void TriggerSubtleShake()
    {
        var source = GameObject.Find("ShakeManager")?.GetComponent<Unity.Cinemachine.CinemachineImpulseSource>();
        if (source != null) source.GenerateImpulse(0.2f);
    }
}