using System;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy_Shooter shooterEnemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openedSprite;
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;

    private List<Enemy_Shooter> spawnedEnemies = new List<Enemy_Shooter>();
    private int totalSpawned = 0;



    private void Start()
    {
        ClearAll();
    }

    private void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (spawnedEnemies.Count > 0) return;
        if (totalSpawned >= enemiesToSpawn) return;

        Enemy_Shooter newEnemy = Instantiate(shooterEnemyPrefab, spawnPoint.position, Quaternion.identity);

        newEnemy.SetSpawner(this);
        newEnemy.SetPatrollPoints(pointA.transform, pointB.transform);
        spawnedEnemies.Add(newEnemy);

        totalSpawned++;
    }

    public void Unregister(Enemy_Shooter obj)
    {
        if (spawnedEnemies.Contains(obj))
        {
            spawnedEnemies.Remove(obj);
        }
    }

    public bool AllEnemiesDefeated()
    {
        return totalSpawned >= enemiesToSpawn && spawnedEnemies.Count == 0;
    }

    private void ClearAll()
    {
        foreach (Enemy_Shooter enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        spawnedEnemies.Clear();
        totalSpawned = 0;
    }

    public void KillAllEnemies()
    {
        foreach (Enemy_Shooter enemy in spawnedEnemies)
        {
            if (enemy)
            {
                Unregister(enemy);
                enemy.TakeDamage(9999);
            }
        }
    }
}
