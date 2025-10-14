using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform sky;
    [SerializeField] private Transform clouds;
    [SerializeField] private Transform mountains;
    [SerializeField] private Transform ground;
    [SerializeField] private float cloudsSpeed = 0.8f;
    [SerializeField] private float mountainsSpeed = 0.6f;


    Transform cam;
    private Vector3 previousCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void Update()
    {
        Vector3 delta = cam.position - previousCamPos;

        if (clouds != null)
            clouds.position += new Vector3(delta.x * cloudsSpeed, 0, 0);

        if (mountains != null)
            mountains.position += new Vector3(delta.x * mountainsSpeed, 0, 0);

        previousCamPos = cam.position;
    }
}
