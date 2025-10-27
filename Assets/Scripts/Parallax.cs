using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private Transform sky;
    [SerializeField] private Transform sky_1;
    [SerializeField] private Transform fog;
    [SerializeField] private Transform fog_1;
    [SerializeField] private Transform mountains;
    [SerializeField] private Transform mountains_1;
    [SerializeField] private Transform ground;
    [SerializeField] private Transform ground_1;

    [Header("Speeds")]
    [SerializeField] private float skySpeed = 0.1f;
    [SerializeField] private float fogSpeed = 0.3f;
    [SerializeField] private float mountainsSpeed = 0.6f;
    [SerializeField] private float groundSpeed = 0f;
    [SerializeField] private float cloudsSpeed = 0.8f;

    [Header("Clouds")]
    [SerializeField] private List<SpriteRenderer> cloudPrefabs = new List<SpriteRenderer>();
    [SerializeField] private Transform cloudsParent;
    [SerializeField] private Vector2 cloudYRange = new Vector2(2f, 6f);
    [SerializeField] private float cloudSpawnDistance = 25f;
    [SerializeField] private float cloudDespawnX = -30f;

    private Transform cam;
    private Vector3 prevCamPos;
    private float nextCloudX;

    void Start()
    {
        cam = Camera.main.transform;
        prevCamPos = cam.position;
        nextCloudX = cam.position.x + cloudSpawnDistance;
    }

    void Update()
    {
        Vector3 delta = cam.position - prevCamPos;

        MoveLayer(sky, sky_1, delta.x, skySpeed);
        MoveLayer(fog, fog_1, delta.x, fogSpeed);
        MoveLayer(mountains, mountains_1, delta.x, mountainsSpeed);
        MoveLayer(ground, ground_1, delta.x, groundSpeed);

        HandleClouds();

        prevCamPos = cam.position;
    }

    void MoveLayer(Transform a, Transform b, float deltaX, float speed)
    {
        a.position += Vector3.left * deltaX * speed;
        b.position += Vector3.left * deltaX * speed;

        float width = a.GetComponent<SpriteRenderer>().bounds.size.x;

        if (cam.position.x - a.position.x > width)
        {
            a.position = new Vector3(b.position.x + width, a.position.y, a.position.z);
        }
        else if (cam.position.x - b.position.x > width)
        {
            b.position = new Vector3(a.position.x + width, b.position.y, b.position.z);
        }
    }

    void HandleClouds()
    {
        if (cam.position.x >= nextCloudX)
        {
            SpawnRandomCloud(nextCloudX + 25);
            nextCloudX += cloudSpawnDistance;
        }

        for (int i = cloudsParent.childCount - 1; i >= 0; i--)
        {
            Transform c = cloudsParent.GetChild(i);
            c.position += Vector3.left * cloudsSpeed * Time.deltaTime;

            if (c.position.x < cam.position.x + cloudDespawnX)
                Destroy(c.gameObject);
        }
    }

    void SpawnRandomCloud(float xPos)
    {
        if (cloudPrefabs.Count == 0) return;

        var prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Count)];
        var cloud = Instantiate(prefab, cloudsParent);

        float yPos = Random.Range(cloudYRange.x, cloudYRange.y);
        cloud.transform.position = new Vector3(xPos, yPos, prefab.transform.position.z);
        cloud.transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
        cloud.color = new Color(1, 1, 1, Random.Range(0.5f, 1f));
    }
}
