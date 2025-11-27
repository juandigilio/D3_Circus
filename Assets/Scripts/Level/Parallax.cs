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
    [SerializeField] private Transform powerPlant;
    [SerializeField] private Transform powerPlant_1;

    [Header("Speeds")]
    [SerializeField] private float skySpeed = 0.1f;
    [SerializeField] private float fogSpeed = 0.3f;
    [SerializeField] private float mountainsSpeed = 0.6f;
    [SerializeField] private float groundSpeed = 0f;
    

    private Transform cam;
    private Vector3 prevCamPos;


    private void Start()
    {
        cam = Camera.main.transform;
        prevCamPos = cam.position;
        
    }

    private void Update()
    {
        Vector3 delta = cam.position - prevCamPos;

        MoveLayer(sky, sky_1, delta.x, skySpeed);
        MoveLayer(fog, fog_1, delta.x, fogSpeed);
        MoveLayer(mountains, mountains_1, delta.x, mountainsSpeed);
        MoveLayer(ground, ground_1, delta.x, groundSpeed);
        MoveLayerOnce(powerPlant, powerPlant_1, delta.x, mountainsSpeed);

        prevCamPos = cam.position;
    }

    private void MoveLayer(Transform a, Transform b, float deltaX, float speed)
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

    private void MoveLayerOnce(Transform a, Transform b, float deltaX, float speed)
    {
        a.position += Vector3.left * deltaX * speed;
        b.position += Vector3.left * deltaX * speed;

       
    }
}
