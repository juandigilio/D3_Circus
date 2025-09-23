using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float screenThreshold = 0.75f;

    private Camera mainCamera;
    private float lastCameraX;

    private void Start()
    {
        mainCamera = Camera.main;
        lastCameraX = mainCamera.transform.position.x;
    }

    private void LateUpdate()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(playerTransform.position);

        if (viewportPos.x > screenThreshold)
        {
            float deltaX = playerTransform.position.x - mainCamera.ViewportToWorldPoint(new Vector3(screenThreshold, viewportPos.y, viewportPos.z)).x;
            float newCameraX = transform.position.x + deltaX;

            transform.position = new Vector3(Mathf.Max(newCameraX, lastCameraX), transform.position.y, transform.position.z);
            lastCameraX = transform.position.x;
        }
    }
}
