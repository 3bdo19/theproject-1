using UnityEngine;
using System.Collections;

public class scripts_manager : MonoBehaviour
{
    [Header("UI Canvases")]
    public GameObject activityCanvas; 
    public GameObject[] allWordScrollers; 

    [Header("Scoring")]
    public int winningScore = 10;
    public int currentScore = 0;

    [Header("Audio Settings")]
    public AudioSource worldBgmSource; 
    public AudioSource sfxSource;      
    
    [Header("Audio Clips")]
    public AudioClip[] worldMusicTracks; 
    public AudioClip openSound;         
    public AudioClip closeSound;       

    [Header("Cat Voice Settings")]
    public AudioClip[] catVoiceSyllables; 

    [Header("NPC Voice Clips")]
    public AudioClip herrSchmidtVoice;
    public AudioClip secondCharacterVoice;
    // The array for your 3 intro stories
    public AudioClip[] introStoryVoices; 

    [Header("Stress Music Settings")]
    public AudioSource stressMusicSource; 
    public AudioClip[] stressMusicTracks;
    public float startVolume = 0.5f;      
    public float volumeIncreaseRate = 0.03f; 
    private Coroutine stressCoroutine;

    [Header("Word Activity References")]
    public WordDestroyer wordDestroyerScript; 

    [Header("End Game UI")]
    public UI gameOverScript;

    [Header("Combat References")]
    public Enemy_respawner enemyRespawner; 

    private bool isGameActive = false;
    private bool anyScrollerWasOpen = false;

    // Standard NPC Voice Playback
    public void PlayCharacterVoice(string characterName)
    {
        if (sfxSource == null) 
        {
            Debug.LogError("SFX Source is missing on scripts_manager!");
            return;
        }

        if (characterName == "HerrSchmidt")
        {
            if (herrSchmidtVoice != null) sfxSource.PlayOneShot(herrSchmidtVoice);
        }
        else if (characterName == "SecondCharacter")
        {
            if (secondCharacterVoice != null) sfxSource.PlayOneShot(secondCharacterVoice);
        }
        else
        {
            Debug.LogWarning("Voice Name '" + characterName + "' not recognized!");
        }
    }

    // NEW: Specific function for Intro Manager to call by index (0, 1, or 2)
    public void PlayIntroVoice(int index)
    {
        if (sfxSource == null) return;

        if (introStoryVoices != null && index < introStoryVoices.Length)
        {
            if (introStoryVoices[index] != null)
                sfxSource.PlayOneShot(introStoryVoices[index]);
            else
                Debug.LogWarning("Intro Voice Clip at index " + index + " is missing!");
        }
    }

    void Start()
    {
        // Don't play music if the intro is currently active
        IntroManager intro = FindObjectOfType<IntroManager>();
        if (intro == null || !intro.introPanel.activeInHierarchy)
        {
            PlayRandomWorldTrack();
        }
    }

    void Update()
    {
        bool anyScrollerCurrentlyOpen = CheckIfAnyScrollerActive();

        if (anyScrollerCurrentlyOpen && !anyScrollerWasOpen)
        {
            if (sfxSource != null) sfxSource.PlayOneShot(openSound);
        }
        else if (!anyScrollerCurrentlyOpen && anyScrollerWasOpen)
        {
            if (sfxSource != null) sfxSource.PlayOneShot(closeSound);
        }

        anyScrollerWasOpen = anyScrollerCurrentlyOpen;

        if (isGameActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (activityCanvas != null && !activityCanvas.activeSelf)
                activityCanvas.SetActive(true);

            if (currentScore >= winningScore)
            {
                StopWordActivity();
            }
        }
    }

    public void StartWordActivity()
    {
        isGameActive = true;
        currentScore = 0; 
        PauseWorldMusic(); 

        if (activityCanvas != null) activityCanvas.SetActive(true);

        if (wordDestroyerScript != null) wordDestroyerScript.enabled = true;

        if (stressMusicSource != null && stressMusicTracks.Length > 0)
        {
            if (stressCoroutine != null) StopCoroutine(stressCoroutine);
            int randomIndex = Random.Range(0, stressMusicTracks.Length);
            stressMusicSource.clip = stressMusicTracks[randomIndex];
            stressCoroutine = StartCoroutine(IncreaseStressVolume());
        }

        if (enemyRespawner != null) enemyRespawner.enabled = true;
    }

    public void StopWordActivity()
    {
        isGameActive = false;
        if (stressCoroutine != null) StopCoroutine(stressCoroutine);
        if (stressMusicSource != null) stressMusicSource.Stop();
        if (activityCanvas != null) activityCanvas.SetActive(false);
        
        if (wordDestroyerScript != null) wordDestroyerScript.enabled = false;

        foreach (GameObject scroller in allWordScrollers)
        {
            if (scroller != null) scroller.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        if (enemyRespawner != null)
        {
            enemyRespawner.enabled = false;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemy in enemies) Destroy(enemy);
        }
        
        ResumeWorldMusic();
    }

    public void PauseWorldMusic()
    {
        if (worldBgmSource != null && worldBgmSource.isPlaying) 
            worldBgmSource.Pause();
    }

    public void ResumeWorldMusic()
    {
        if (worldBgmSource != null && !worldBgmSource.isPlaying) 
            worldBgmSource.UnPause();
    }

    public void PlayRandomWorldTrack()
    {
        if (worldMusicTracks.Length > 0 && worldBgmSource != null)
        {
            int randomIndex = Random.Range(0, worldMusicTracks.Length);
            worldBgmSource.clip = worldMusicTracks[randomIndex];
            worldBgmSource.loop = true;
            worldBgmSource.Play();
        }
    }

    bool CheckIfAnyScrollerActive()
    {
        foreach (GameObject scroller in allWordScrollers)
        {
            if (scroller != null && scroller.activeSelf) return true;
        }
        return false;
    }

    public void PlayRandomCatVoiceOnce()
    {
        if (sfxSource != null && catVoiceSyllables.Length > 0)
        {
            int randomIndex = Random.Range(0, catVoiceSyllables.Length);
            sfxSource.PlayOneShot(catVoiceSyllables[randomIndex]);
        }
    }

    public void StopCatVoice()
    {
        if (sfxSource != null) sfxSource.Stop();
    }

    IEnumerator IncreaseStressVolume()
    {
        if (stressMusicSource == null) yield break;
        stressMusicSource.volume = startVolume;
        stressMusicSource.loop = true; 
        stressMusicSource.Play();

        while (isGameActive && stressMusicSource.volume < 1.0f)
        {
            yield return new WaitForSeconds(1f);
            stressMusicSource.volume += volumeIncreaseRate;
        }
    }

    public void TriggerGameOver()
    {
        isGameActive = false;
        if (stressMusicSource != null) stressMusicSource.Stop();
        if (stressCoroutine != null) StopCoroutine(stressCoroutine);
        if (wordDestroyerScript != null) wordDestroyerScript.enabled = false;
        if (gameOverScript != null) gameOverScript.EnableGameOverUI(); 

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}