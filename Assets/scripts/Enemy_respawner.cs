
using UnityEngine;

public class Enemy_respawner : MonoBehaviour
{   
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float cooldown;
    private float timer;



    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0,respawnPoints.Length);
        Vector3 spawnPoint =  respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab,spawnPoint, Quaternion.identity);
    }
}
