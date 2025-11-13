using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItmesSpawner : MonoBehaviour
{
    [Header("Baloon Prefabs")]
    [SerializeField] private Baloon smallHealth;
    [SerializeField] private Baloon bigHealth;
    [SerializeField] private Baloon machinegunAmmo;
    [SerializeField] private Baloon rifleAmmo;
    [SerializeField] private Baloon smallCoin;
    [SerializeField] private Baloon bigCoin;

    [Header("Spawn Settings")]
    [SerializeField] private float healthSpawnInterval = 10f;
    [SerializeField] private float coinSpawnInterval = 30f;


    private PlayerController playerController;
    private List<Transform> spawnPoints = new List<Transform>();
    private float lastHealthSpawnTime = 0f;
    private float lastAmmoSpawnTime = 0f;
    private float lastCoinSpawnTime = 0f;
    private bool wasBigCoin = false;
    private bool isPaused = false;


    private void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();

        spawnPoints.Clear();
        spawnPoints.AddRange(GameManager.Instance.GetSideScrollCamera().GetSideSpawnPoints());

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
    }

    private void FixedUpdate()
    {
        if (isPaused) return;

        CheckHealth();
        CheckAmmo();
        SpawnCoin();
    }

    private void OnDestroy()
    {
        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
    }

    private void SpawnCoin()
    {
        lastCoinSpawnTime += Time.fixedDeltaTime;

        if (lastCoinSpawnTime < coinSpawnInterval)
            return;

        if (wasBigCoin)
        {
            SpawnBaloon(smallCoin);
        }
        else
        {
            SpawnBaloon(bigCoin);
        }

        wasBigCoin = !wasBigCoin;
        lastCoinSpawnTime = 0f;
    }

    private void CheckHealth()
    {
        lastHealthSpawnTime += Time.fixedDeltaTime;

        if (lastHealthSpawnTime < healthSpawnInterval)
            return;

        if (playerController.CurrentHealth() < playerController.MaxHealth() * 0.3f)
        {
            SpawnBaloon(bigHealth);
            lastHealthSpawnTime = 0f;
        }
        else if (playerController.CurrentHealth() < playerController.MaxHealth() * 0.7f)
        {
            SpawnBaloon(smallHealth);
            lastHealthSpawnTime = 0f;
        }
    }

    private void CheckAmmo()
    {
        lastAmmoSpawnTime += Time.fixedDeltaTime;

        if (lastAmmoSpawnTime < healthSpawnInterval)
            return;

        if (playerController.GetWeaponsManager().GetMachineGunAmmo() < 30)
        {
            SpawnBaloon(machinegunAmmo);
            lastAmmoSpawnTime = 0f;
        }
        else if (playerController.GetWeaponsManager().GetRifleAmmo() < 15)
        {
            SpawnBaloon(rifleAmmo);
            lastAmmoSpawnTime = 0f;
        }
    }

    private void SpawnBaloon(Baloon baloon)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Count);
        Instantiate(baloon, spawnPoints[spawnIndex].position, Quaternion.identity);
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
