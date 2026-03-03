using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WordDestroyer : MonoBehaviour
{
    [Header("UI & Spawning")]
    public GameObject wordPrefab; 
    public Transform worldSpawnAnchor; 
    public float fallSpeed = 5f; // Reduced for world space (100f is too fast for 2D)
    public GameObject activityCanvas;
    public TextMeshProUGUI scoreText;

    [Header("Game Logic")]
    public string[] targetWords; 
    public string[] decoyWords;  
    public int pointsToWin = 10;
    
    [Header("Room Transition")]
    public Animator doorAnimator; 

    private int currentScore = 0;
    private float spawnTimer;
    
    [HideInInspector] public bool isGameActive = false; 
    public scripts_manager myManager;

    // --- THE FIX: Reset everything when the manager enables this script ---
    private void OnEnable()
    {
        Debug.Log("WordDestroyer: Script Enabled!"); // Check 1
        isGameActive = true;
        currentScore = 0;
        spawnTimer = 0.5f; // Initial delay
        
        if (scoreText != null) scoreText.text = "Score: 0";
    }

    void Update()
    {
        if (!isGameActive) return;

        spawnTimer -= Time.deltaTime;
        
        if (spawnTimer <= 0)
        {
            Debug.Log("WordDestroyer: Attempting to Spawn..."); // Check 2
            SpawnWord();
            spawnTimer = 1.5f; 
        }
    }

    void SpawnWord()
    {
        if (worldSpawnAnchor == null) return;

        float roomWidth = 8.0f; 
        float randomX = worldSpawnAnchor.position.x + Random.Range(-roomWidth / 2, roomWidth / 2);

        GameObject newWord = Instantiate(wordPrefab, new Vector3(randomX, worldSpawnAnchor.position.y, 0), Quaternion.identity);
        
        float randomSpeed = Random.Range(fallSpeed * 0.8f, fallSpeed * 1.2f);
        
        // Use a 50/50 chance for target vs decoy
        bool isTarget = Random.value > 0.5f;
        string txt = isTarget ? targetWords[Random.Range(0, targetWords.Length)] : decoyWords[Random.Range(0, decoyWords.Length)];
        
        // Ensure the FallingWord script gets the data
        if(newWord.TryGetComponent(out FallingWord fw))
        {
            fw.Setup(txt, isTarget, this, randomSpeed);
        }
    }

    public void WordClicked(bool isTarget)
    {
        if (!isGameActive) return;

        if (isTarget)
        {
            currentScore++;
            if (myManager != null) myManager.currentScore++;
        }
        else
        {
            currentScore = Mathf.Max(0, currentScore - 1);
            if (myManager != null) myManager.currentScore--;
        }

        if (scoreText != null) scoreText.text = "Score: " + currentScore;

        if (currentScore >= pointsToWin) WinActivity();
    }

    void WinActivity()
    {
        isGameActive = false; 
        
        // 1. Tell the manager to stop the stress music and UI
        if (myManager != null)
        {
            myManager.StopWordActivity();
        }

        // 2. Handle the door
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("Open"); 
            if (doorAnimator.TryGetComponent(out Collider2D col)) col.enabled = false;
        }

        // 3. Clear any remaining words in the scene
        FallingWord[] remainingWords = FindObjectsOfType<FallingWord>();
        foreach (FallingWord w in remainingWords) Destroy(w.gameObject);
    }
}