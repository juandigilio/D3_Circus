using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private int totalEnemies;
    [SerializeField] private Transform[] spawnPoints;

    private float lastSpawn;
    private int enemiesSpawned;
    private bool isPaused = false;


    private void Start()
    {
        lastSpawn = Time.time;

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
        MenuController.OnGameStarted += StopPause;
    }

    private void OnDestroy()
    {
        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
        MenuController.OnGameStarted -= StopPause;
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            SpawnEnemies();
        }   
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

    private void SetPaused()
    {
        isPaused = true;
    }

    private void StopPause()
    {
        isPaused = false;
    }
}
