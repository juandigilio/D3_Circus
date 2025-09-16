using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private int totalEnemies;
    [SerializeField] private Transform[] spawnPoints;

    private float lastSpawn;
    private int enemiesSpawned;



    private void Start()
    {
        lastSpawn = Time.time;
    }

    private void FixedUpdate()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (Time.time > spawnRate + lastSpawn && enemiesSpawned < totalEnemies)
        {
            lastSpawn = Time.time;
            enemiesSpawned++;

            int randomIndex = Random.Range(0, spawnPoints.Length);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        }
    }
}
