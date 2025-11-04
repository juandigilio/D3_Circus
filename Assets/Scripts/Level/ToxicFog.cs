using System.Collections.Generic;
using UnityEngine;

public class ToxicFog : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [Header("Clouds")]
    [SerializeField] private List<SpriteRenderer> cloudPrefabs = new List<SpriteRenderer>();
    [SerializeField] private Transform cloudsParent;
    [SerializeField] private float cloudYMinRange = 0.5f;
    [SerializeField] private float cloudYMaxRange = 6f;
    [SerializeField] private float cloudSpawnDistance = 25f;
    [SerializeField] private float cloudDespawnX = -30f;
    [SerializeField] private float baseCloudSpeed = 0.8f;
    [SerializeField] private float cloudSpeedVariation = 0.4f;

    [Header("Spawn Control")]
    [SerializeField] private float cloudSpawnInterval = 5f;
    private float cloudSpawnTimer = 0f;

    [Header("Damage")]
    [SerializeField] private float damagePerSecond = 0.2f;
    [SerializeField] private float toxicPercent;
    [SerializeField] private float minToxicPercent = 0.3f;

    private PlayerController player;
    private Transform cam;
    private float nextCloudX;
    private float levelTime;
    private float restingTime;
    private float damageTimer = 1f;
    private Color cloudColor;
    private bool isPaused = false;

    private void Start()
    {
        player = GameManager.Instance.GetPlayerController();
        cam = Camera.main.transform;

        nextCloudX = cam.position.x + cloudSpawnDistance;
        levelTime = levelManager.GetLevelTime();
        restingTime = levelManager.GetTotalTime();
        toxicPercent = 1f - (restingTime / levelTime);

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
    }

    

    private void FixedUpdate()
    {
        HandleClouds();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPaused) return;

        if (collision.CompareTag("Player"))
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0f)
            {
                player.TakeDamage(damagePerSecond * toxicPercent);
                damageTimer = 1f;
            }
        }
    }

    private void HandleClouds()
    {
        if (isPaused) return;

        MoveClouds();

        HandleCloudsSpawn();
    }

    private void HandleCloudsSpawn()
    {
        if (cam.position.x >= nextCloudX)
        {
            UpdateToxicPercen();
            nextCloudX += cloudSpawnDistance * toxicPercent;
            SpawnRandomCloud(nextCloudX);
        }

        cloudSpawnTimer += Time.fixedDeltaTime;

        if (cloudSpawnTimer >= (cloudSpawnInterval / toxicPercent))
        {
            cloudSpawnTimer = 0f;
            UpdateToxicPercen();
            float spawnX = cam.position.x + (cloudSpawnDistance * toxicPercent);
            SpawnRandomCloud(spawnX);
        }
    }

    private void MoveClouds()
    {
        for (int i = cloudsParent.childCount - 1; i >= 0; i--)
        {
            Transform c = cloudsParent.GetChild(i);

            CloudData data = c.GetComponent<CloudData>();
            if (data == null) continue;

            c.position += Vector3.left * data.speed * Time.deltaTime;

            float windY = Mathf.Sin(Time.time * data.windFrequency + data.randomOffset) * data.windAmplitude;
            c.position = new Vector3(c.position.x, data.baseY + windY, c.position.z);

            if (c.position.x < cam.position.x + cloudDespawnX)
                Destroy(c.gameObject);
        }
    }

    private void SpawnRandomCloud(float xPos)
    {
        if (cloudPrefabs.Count == 0) return;

        SpriteRenderer prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
        SpriteRenderer cloud = Instantiate(prefab, cloudsParent);

        float yPos = Random.Range(cloudYMinRange, cloudYMaxRange);
        cloud.transform.position = new Vector3(xPos, yPos, cloudsParent.transform.position.z);
        cloud.transform.localScale = new Vector3(toxicPercent, toxicPercent, 1);
        cloud.color = cloudColor;

        CloudData data = cloud.gameObject.AddComponent<CloudData>();
        data.speed = baseCloudSpeed + Random.Range(-cloudSpeedVariation, cloudSpeedVariation);
        data.baseY = yPos;
        data.windFrequency = Random.Range(0.4f, 1.5f);
        data.windAmplitude = Random.Range(0.05f, 0.25f);
        data.randomOffset = Random.Range(0f, 10f);

        data.speed *= (1f - (toxicPercent * 0.2f));
    }

    private void UpdateToxicPercen()
    {
        restingTime = levelManager.GetTotalTime();
        toxicPercent = 1f - (restingTime / levelTime);

        if (toxicPercent < minToxicPercent)
            toxicPercent = minToxicPercent;
     
        cloudColor = new Color(0, 1, 0, toxicPercent * 2);
    }

    private void SetPaused()
    {
        isPaused = true;
    }

    private void StopPause()
    {
        isPaused = false;
    }

    private class CloudData : MonoBehaviour
    {
        public float speed;
        public float baseY;
        public float windFrequency;
        public float windAmplitude;
        public float randomOffset;
    }
}
