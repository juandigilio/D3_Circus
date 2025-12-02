using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumperEnemiesManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Enemy_Jumper enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int maxAttackers = 2;
    [SerializeField] private int maxActiveJumpers = 5;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private GameObject spawnParent;
    [SerializeField] private List<int> enemiesToSpawnPerSector = new List<int>();


    private List<Enemy_Jumper> jumpers = new List<Enemy_Jumper>();
    private List<Enemy_Jumper> activeAttacking = new List<Enemy_Jumper>();
    private List<Transform> outterPoints = new List<Transform>();

    private List<Transform> spawnPoints = new List<Transform>();
    private int nextAttacker = 0;
    private int spawnedEnemies = 0;
    private int currentSector = 0;
    private float lastAdded = 0;
    private float addingRate = 1.0f;

    private float lastSpawn;


    private void Awake()
    {
        GameManager.Instance.RegisterJumperEnemiesManager(this);

        ClearLists();
        lastSpawn = 0f;
        nextAttacker = 0;
        currentSector = 0;

        spawnPoints.AddRange(GameManager.Instance.GetSideScrollCamera().GetJumperUpperPoints());
        outterPoints.AddRange(GameManager.Instance.GetSideScrollCamera().GetJumperLateralPoints());
    }

    private void FixedUpdate()
    {
        if (player == null || GameManager.Instance.GetPlayerController().IsPaused()) return;

        SpawnEnemies();
        HandleJumpers();
    }

    public void KillAll()
    {
        foreach (Enemy_Jumper jumper in spawnParent.GetComponentsInChildren<Enemy_Jumper>())
        {
            jumper.TakeDamage(9999);
        }
    }

    public void LoadNextSector()
    {
        ClearLists();

        if (currentSector + 1 >= enemiesToSpawnPerSector.Count)
        {
            return;
        }

        currentSector++;
    }

    public void Register(Enemy_Jumper jumper)
    {
        if (!jumpers.Contains(jumper))
        {
            jumpers.Add(jumper);
        }
    }

    public void Unregister(Enemy_Jumper jumper)
    {
        if (jumpers.Contains(jumper))
        {
            jumpers.Remove(jumper);
        }
        if (activeAttacking.Contains(jumper))
        {
            activeAttacking.Remove(jumper);
        }
    }

    public void NotifyAttack(Enemy_Jumper jumper)
    {
        if (activeAttacking.Contains(jumper))
        {
            activeAttacking.Remove(jumper);
        }
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    public bool IsCleared()
    {
        if (spawnedEnemies < enemiesToSpawnPerSector[currentSector]) return false;

        int deadJumpers = 0;

        foreach (Enemy_Jumper jumper in spawnParent.GetComponentsInChildren<Enemy_Jumper>())
        {
            if (jumper.IsDead())
            {
                deadJumpers++;
            }
        }

        if (deadJumpers >= spawnParent.GetComponentsInChildren<Enemy_Jumper>().Count())
            return true;

        foreach (Enemy_Jumper jumper in spawnParent.GetComponentsInChildren<Enemy_Jumper>())
        {
            if (jumper != null)
            {
                return false;
            }
        }
        return true;
    }

    private void HandleJumpers()
    {
        if (activeAttacking.Count < maxAttackers && jumpers.Count > 0)
        {
            lastAdded += Time.fixedDeltaTime;
            if (lastAdded < addingRate) return;

            lastAdded = 0f;

            if (nextAttacker >= jumpers.Count)
            {
                nextAttacker = 0;
            }

            if (!activeAttacking.Contains(jumpers[nextAttacker]))
            {
                jumpers[nextAttacker].StartChase();
                activeAttacking.Add(jumpers[nextAttacker]);
            }

            nextAttacker = (nextAttacker + 1) % jumpers.Count;
        }
    }

    private void SpawnEnemies()
    {
        if (spawnedEnemies >= enemiesToSpawnPerSector[currentSector]) return;

        lastSpawn += Time.fixedDeltaTime;

        if (lastSpawn >= spawnRate && jumpers.Count < maxActiveJumpers)
        {
            lastSpawn = 0f;
            spawnedEnemies++;

            int randomIndex = Random.Range(0, spawnPoints.Count);
            Enemy_Jumper newEnemy = Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity, spawnParent.transform);

            jumpers.Add(newEnemy);
            newEnemy.StartChase();
            activeAttacking.Add(newEnemy);
            nextAttacker++;
        }
    }

    private void ClearLists()
    {
        foreach (Enemy_Jumper jumper in jumpers)
        {
            Destroy(jumper);
        }

        foreach (Enemy_Jumper jumper in activeAttacking)
        {
            Destroy(jumper);
        }

        jumpers.Clear();
        activeAttacking.Clear();
        //outterPoints.Clear();
        //spawnPoints.Clear();

        nextAttacker = 0;
        spawnedEnemies = 0;
        lastSpawn = 0f;
    }
}