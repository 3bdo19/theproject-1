using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WordDestroyer : MonoBehaviour
{
    [Header("UI & Spawning")]
    public GameObject wordPrefab; // A button or UI element with a script
    public Transform worldSpawnAnchor; 
    public float fallSpeed = 100f;
    public GameObject activityCanvas;
    public TextMeshProUGUI scoreText;

    [Header("Game Logic")]
    public string[] targetWords; // Words the player MUST click
    public string[] decoyWords;  // Words to distract the player
    public int pointsToWin = 10;
    
    [Header("Room Transition")]
    public Animator doorAnimator; // Change this from GameObject to Animator

    private int currentScore = 0;
    private float spawnTimer;
     // Drag your ScoreText UI object here in Inspector
    public bool isGameActive = false; // Add this at the top!

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnWord();
            spawnTimer = 1.5f; // Adjust for difficulty
        }
    }

    void SpawnWord()
{
    // 1. Calculate random X based on the WorldSpawnArea's width
    // Define the width of your room's 'drop zone'
    float roomWidth = 8.0f; 
    float randomX = worldSpawnAnchor.position.x + Random.Range(-roomWidth / 2, roomWidth / 2);

    // Instantiate in the World (Z = 0 is where the player is)
    GameObject newWord = Instantiate(wordPrefab, new Vector3(randomX, worldSpawnAnchor.position.y, 0), Quaternion.identity);
    
    
    // 3. Setup the word
    float randomSpeed = Random.Range(fallSpeed * 0.8f, fallSpeed * 1.2f);
    bool isTarget = Random.value > 0.5f;
    string txt = isTarget ? targetWords[Random.Range(0, targetWords.Length)] : decoyWords[Random.Range(0, decoyWords.Length)];
    
    newWord.GetComponent<FallingWord>().Setup(txt, isTarget, this, randomSpeed);
}

    public void WordClicked(bool isTarget)
{
    if (isTarget)
    {
        currentScore++;
    }
    else
    {
        currentScore = Mathf.Max(0, currentScore - 1);
    }

    // THIS LINE IS CRUCIAL:
    if (scoreText != null) 
    {
        scoreText.text = "Score: " + currentScore;
    }

    if (currentScore >= pointsToWin) WinActivity();
}

   void WinActivity()
{
    isGameActive = false; // This stops words from spawning
    
    if (doorAnimator != null)
    {
        // 1. Play the animation
        doorAnimator.SetTrigger("Open"); 
        
        // 2. Disable the door's collider so you can walk through
        if (doorAnimator.TryGetComponent(out Collider2D col)) col.enabled = false;
    }

    // 3. Start a timer to hide the UI so the player can see the door open
    Invoke("HideUI", 1.5f); 
}

void HideUI()
{
    activityCanvas.SetActive(false); // Hide the manager
    // If your Canvas is a separate object, use: activityCanvas.SetActive(false);
}
}