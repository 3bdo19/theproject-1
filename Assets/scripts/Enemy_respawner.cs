using UnityEngine;

public class Enemy_respawner : MonoBehaviour
{   
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float initialCooldown = 2f; 
    private float cooldown;

    [Space]
    [SerializeField] private float CooldownDecreaseRate = .05f;
    [SerializeField] private float CooldownCap = .7f;
    
    private float timer;
    private Transform player;

    void Awake()
    {
        Player playerComponent = FindAnyObjectByType<Player>();
        if (playerComponent != null) player = playerComponent.transform;
        
        cooldown = initialCooldown;
    }

    private void OnEnable()
    {
        timer = 0f; 
        
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

           
            cooldown = Mathf.Max(CooldownCap, cooldown - CooldownDecreaseRate);
        }
    }

    private void CreateNewEnemy()
    {
        if (player == null || respawnPoints.Length == 0) return;

        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPoint = respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        
        bool CreatedOnTheRight = newEnemy.transform.position.x > player.position.x;

        if (CreatedOnTheRight)
        {
            
            if (newEnemy.TryGetComponent(out enemy enemyScript))
            {
                enemyScript.flip();
            }
        }
    }
}