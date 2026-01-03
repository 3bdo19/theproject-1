
using UnityEngine;

public class Enemy_respawner : MonoBehaviour
{   
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown = 2;
    [Space]
    [SerializeField] private float CooldownDecreaseRate = .05f;
    [SerializeField] private float CooldownCap = .7f;
    private float timer;
    private Transform player;

    void Awake()
    {
        player = FindAnyObjectByType<Player>().transform;

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
        int respawnPointIndex = Random.Range(0,respawnPoints.Length);
        Vector3 spawnPoint =  respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab,spawnPoint, Quaternion.identity);

        bool CreatedOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if (CreatedOnTheRight)
        {
            newEnemy.GetComponent<enemy>().flip();
        }
    }
}
