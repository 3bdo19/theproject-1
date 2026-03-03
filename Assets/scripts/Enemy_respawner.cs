using UnityEngine;

public class Enemy_respawner : MonoBehaviour
{   
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float initialCooldown = 2f; // Store the original value
    private float cooldown;

    [Space]
    [SerializeField] private float CooldownDecreaseRate = .05f;
    [SerializeField] private float CooldownCap = .7f;
    
    private float timer;
    private Transform player;

    void Awake()
    {
        // Finding the player transform
        Player playerComponent = FindAnyObjectByType<Player>();
        if (playerComponent != null) player = playerComponent.transform;
        
        cooldown = initialCooldown;
    }

    // --- THE FIX: This runs every time the script is enabled by the manager ---
    private void OnEnable()
    {
        // 1. Reset timer so an enemy spawns immediately
        timer = 0f; 
        
        // 2. Reset the cooldown speed so it doesn't stay super fast from the last game
        cooldown = initialCooldown; 
        
        Debug.Log("Enemy Spawning Active!");
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();

            // Make the game progressively harder
            cooldown = Mathf.Max(CooldownCap, cooldown - CooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        if (player == null || respawnPoints.Length == 0) return;

        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        // Check if enemy is on the right to flip the sprite
        bool CreatedOnTheRight = newEnemy.transform.position.x > player.position.x;

        if (CreatedOnTheRight)
        {
            // Assuming your enemy script has a flip function
            if (newEnemy.TryGetComponent(out enemy enemyScript))
            {
                enemyScript.flip();
            }
        }
    }
}