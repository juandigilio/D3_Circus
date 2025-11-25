using System.Collections.Generic;
using UnityEngine;

public class JumperEnemiesManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Enemy_Jumper enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int maxAttackers = 2;
    [SerializeField] private int maxActiveJumpers = 5;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private GameObject spawnParent;


    private List<Enemy_Jumper> jumpers = new List<Enemy_Jumper>();
    private List<Enemy_Jumper> activeAttacking = new List<Enemy_Jumper>();
    private List<Transform> outterPoints = new List<Transform>();

    private List<Transform> spawnPoints = new List<Transform>();
    private int nextAttacker = 0;
    private int spawnedEnemies = 0;

    private float lastSpawn;


    private void Awake()
    {
        GameManager.Instance.RegisterJumperEnemiesManager(this);

        ClearLists();
        lastSpawn = 0f;
        nextAttacker = 0;

        spawnPoints.AddRange(GameManager.Instance.GetSideScrollCamera().GetJumperUpperPoints());
        outterPoints.AddRange(GameManager.Instance.GetSideScrollCamera().GetJumperLateralPoints());
    }

    private void FixedUpdate()
    {
        if (player == null || GameManager.Instance.GetPlayerController().IsPaused()) return;

        SpawnEnemies();
        HandleJumpers();

        if (spawnedEnemies >= enemiesToSpawn)
        {
            levelManager.NotifyJumperEnemiesCleared();
        }
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

    private void HandleJumpers()
    {
        if (activeAttacking.Count < maxAttackers && jumpers.Count > 0)
        {
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
        if (spawnedEnemies >= enemiesToSpawn) return;

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

        jumpers.Clear();
        activeAttacking.Clear();
        outterPoints.Clear();
        spawnPoints.Clear();

        nextAttacker = 0;
        spawnedEnemies = 0;
        lastSpawn = 0f;
    }

    public void KillAllJumpers()
    {
        foreach (Enemy_Jumper jumper in spawnParent.GetComponentsInChildren<Enemy_Jumper>())
        {
            jumper.TakeDamage(9999);
        }
    }
}