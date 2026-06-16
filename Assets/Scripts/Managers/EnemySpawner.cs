using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnRate = 1.5f;
    public float spawnDistance = 12f;
    public int maxEnemiesAlive = 40;

    private float spawnTimer;
    private readonly List<GameObject> aliveEnemies = new List<GameObject>();

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (enemyPrefab == null || player == null)
            return;

        RemoveDeadEnemies();

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate && aliveEnemies.Count < maxEnemiesAlive)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetSpawnPosition();

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        Vector3 spawnPosition = player.position + new Vector3(
            randomDirection.x,
            randomDirection.y,
            0f
        ) * spawnDistance;

        return spawnPosition;
    }

    private void RemoveDeadEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] == null)
            {
                aliveEnemies.RemoveAt(i);
            }
        }
    }
}