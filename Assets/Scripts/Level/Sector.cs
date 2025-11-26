using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    [SerializeField] private List<ShooterEnemySpawner> spawners = new List<ShooterEnemySpawner>();
    [SerializeField] private List<Enemy_Shooter> shooters = new List<Enemy_Shooter>();
    [SerializeField] private List<Cage> cages = new List<Cage>();


    public bool IsSectorCleared()
    {
        foreach (ShooterEnemySpawner spawner in spawners)
        {
            if (!spawner.AllEnemiesDefeated())
            {
                return false;
            }
        }
        foreach (Enemy_Shooter shooter in shooters)
        {
            if (shooter)
            {
                return false;
            }
        }
        return true;
    }

    public void KillAll()
    {
        foreach (Enemy_Shooter shooter in shooters)
        {
            if (shooter)
            {
                shooter.TakeDamage(9999);
            }
        }

        foreach (ShooterEnemySpawner spawner in spawners)
        {
            spawner.KillAllEnemies();
        }
    }

    public List<Cage> GetCages()
    {
        return cages;
    }
}
