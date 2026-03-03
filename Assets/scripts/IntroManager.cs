using UnityEngine;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject introPanel;
    public TextMeshProUGUI[] storyTextObjects = new TextMeshProUGUI[3]; 
    public GameObject skipNextButton; 

    [Header("Settings")]
    [TextArea(3, 10)] 
    public string[] stories = new string[3]; 
    
    // 1. Array for 3 different typing speeds
    public float[] typingSpeeds = new float[3] { 0.05f, 0.05f, 0.05f }; 

    private int storyIndex = 0;
    private Coroutine typingCoroutine;
    private bool isCurrentlyTyping = false; 
    private static bool hasSeenIntro = false; 

    void Awake()
    {
        if (hasSeenIntro)
        {
            introPanel.SetActive(false);
            return;
        }

        scripts_manager manager = FindObjectOfType<scripts_manager>();
        if (manager != null) manager.PauseWorldMusic();
    }

    void Start()
    {
        if (!hasSeenIntro)
        {
            foreach (var textObj in storyTextObjects) textObj.gameObject.SetActive(false);
            skipNextButton.SetActive(true); 
            typingCoroutine = StartCoroutine(PlayStory(storyIndex));
        }
    }

    IEnumerator PlayStory(int index)
    {
        isCurrentlyTyping = true;
        storyTextObjects[index].gameObject.SetActive(true);
        storyTextObjects[index].text = "";

        scripts_manager manager = FindObjectOfType<scripts_manager>();
        if (manager != null) manager.PlayIntroVoice(index);

        // 2. Use the speed from the array based on the index
        float currentSpeed = typingSpeeds[index]; 

        foreach (char letter in stories[index].ToCharArray())
        {
            storyTextObjects[index].text += letter;
            yield return new WaitForSeconds(currentSpeed);
        }

        if (manager != null) manager.StopCatVoice();
        isCurrentlyTyping = false;
    }

    public void HandleButtonClick()
    {
        scripts_manager manager = FindObjectOfType<scripts_manager>();

        if (isCurrentlyTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            storyTextObjects[storyIndex].text = stories[storyIndex];
            if (manager != null) manager.StopCatVoice();
            isCurrentlyTyping = false;
            return; 
        }

        storyTextObjects[storyIndex].gameObject.SetActive(false);
        storyIndex++;

        if (storyIndex < stories.Length)
        {
            typingCoroutine = StartCoroutine(PlayStory(storyIndex));
        }
        else
        {
            SkipIntro();
        }
    }

public void SkipIntro()
{
    hasSeenIntro = true; 
    
    scripts_manager manager = FindObjectOfType<scripts_manager>();
    if (manager != null)
    {
        manager.StopCatVoice(); // Stop any leftover story audio
        
        // NEW LOGIC: Check if music is actually playing
        if (manager.worldBgmSource != null && manager.worldBgmSource.clip == null)
        {
            // If no music was ever started, start it now
            manager.PlayRandomWorldTrack(); 
        }
        else
        {
            // If music was paused, resume it
            manager.ResumeWorldMusic(); 
        }
    }

    introPanel.SetActive(false);
}
}   