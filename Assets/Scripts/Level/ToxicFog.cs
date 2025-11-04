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
    [SerializeField] private float cloudsSpeed = 0.8f;

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

    private void Start()
    {
        player = GameManager.Instance.GetPlayerController();

        cam = Camera.main.transform;
        nextCloudX = cam.position.x + (cloudSpawnDistance * minToxicPercent);
        levelTime = levelManager.GetLevelTime();
        restingTime = levelManager.GetTotalTime();
        toxicPercent = 1f - (restingTime / levelTime);
    }

    private void FixedUpdate()
    {
        HandleClouds();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
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
        if (cam.position.x >= nextCloudX)
        {
            UpdateToxicPercen();
            SpawnRandomCloud(nextCloudX);

            Debug.Log("Spawned cloud at x: " + (nextCloudX - cloudSpawnDistance));
        }

        for (int i = cloudsParent.childCount - 1; i >= 0; i--)
        {
            Transform c = cloudsParent.GetChild(i);
            c.position += Vector3.left * cloudsSpeed * Time.deltaTime;

            if (c.position.x < cam.position.x + cloudDespawnX)
                Destroy(c.gameObject);
        }
    }

    private void SpawnRandomCloud(float xPos)
    {
        if (cloudPrefabs.Count == 0) return;

        SpriteRenderer prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
        SpriteRenderer cloud = Instantiate(prefab, cloudsParent);

        float yPos = Random.Range((cloudYMinRange / toxicPercent), cloudYMaxRange);
        cloud.transform.position = new Vector3(xPos, yPos, prefab.transform.position.z);
        cloud.transform.localScale = new Vector3(toxicPercent, toxicPercent, 1);
        cloud.color = cloudColor;

        Debug.Log("Cloud scale: " + cloud.transform.localScale.ToString());
    }

    private void UpdateToxicPercen()
    {
        restingTime = levelManager.GetTotalTime();

        toxicPercent = 1f - (restingTime / levelTime);

        //Debug.Log("Toxic Percent: " + toxicPercent);
        //Debug.Log("Rsting time: " + restingTime);
        

        if (toxicPercent < 0.3f)
            toxicPercent = 0.3f;

        nextCloudX += cloudSpawnDistance * toxicPercent;
        cloudColor = new Color(toxicPercent, toxicPercent, toxicPercent, toxicPercent);
    }
}
