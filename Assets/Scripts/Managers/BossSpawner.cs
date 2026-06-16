using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject bossPrefab;
    public Transform player;

    [Header("Boss Settings")]
    public float bossSpawnTime = 30f;
    public float spawnDistance = 14f;
    public int maxBossesPerLevel = 1;

    private float timer;
    private int spawnedBosses;

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
        if (bossPrefab == null || player == null)
            return;

        if (spawnedBosses >= maxBossesPerLevel)
            return;

        timer += Time.deltaTime;

        if (timer >= bossSpawnTime)
        {
            SpawnBoss();
            spawnedBosses++;
        }
    }

    private void SpawnBoss()
    {
        Vector3 spawnPosition = GetSpawnPosition();

        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Boss Spawned!");
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
}