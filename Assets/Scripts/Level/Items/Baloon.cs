using UnityEngine;

public class Baloon : MonoBehaviour
{
    [SerializeField] private Item itemPrefab;

    [Header("Float Settings")]
    [SerializeField] private float horizontalSpeed = 1.5f;
    [SerializeField] private float verticalAmplitude = 0.3f;
    [SerializeField] private float verticalFrequency = 1.2f;
    [SerializeField] private float swayAmplitude = 0.5f;
    [SerializeField] private float swayFrequency = 0.7f;


    private Camera mainCam;
    private Vector3 startPos;
    private float randomOffset;

    private void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 1f);
        mainCam = Camera.main;
    }

    private void Update()
    {
        CheckOffScreen();
        FloatMovement();
    }

    public void Pop()
    {
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void FloatMovement()
    {
        float x = transform.position.x - horizontalSpeed * Time.deltaTime;
        float y = startPos.y + Mathf.Sin(Time.time * verticalFrequency + randomOffset) * verticalAmplitude;
        float sway = Mathf.Sin(Time.time * swayFrequency + randomOffset) * swayAmplitude;

        transform.position = new Vector3(x - sway, y, transform.position.z);
    }

    private void CheckOffScreen()
    {
        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);

        if (viewportPos.x < -0.2f)
        {
            Destroy(gameObject);
        }
    }
}
