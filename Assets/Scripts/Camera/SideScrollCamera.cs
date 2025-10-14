using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] private float screenThresholdX = 0.75f;
    [SerializeField] private float upperThresholdY = 0.7f;
    [SerializeField] private float lowerThresholdY = 0.35f;

    private Transform playerTransform;
    private Camera mainCamera;
    private float lastCameraX;

    private void Start()
    {
        mainCamera = Camera.main;
        lastCameraX = mainCamera.transform.position.x;
        GameManager.Instance.RegisterSideSrollCamera(this);
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(playerTransform.position);
            Vector3 newCamPos = transform.position;

            if (viewportPos.x > screenThresholdX)
            {
                float deltaX = playerTransform.position.x - mainCamera.ViewportToWorldPoint(new Vector3(screenThresholdX, viewportPos.y, viewportPos.z)).x;
                float newCameraX = transform.position.x + deltaX;
                newCamPos.x = Mathf.Max(newCameraX, lastCameraX);
            }

            if (viewportPos.y > upperThresholdY)
            {
                float deltaY = playerTransform.position.y - mainCamera.ViewportToWorldPoint(new Vector3(viewportPos.x, upperThresholdY, viewportPos.z)).y;
                newCamPos.y += deltaY;
            }
            else if (viewportPos.y < lowerThresholdY)
            {
                float deltaY = playerTransform.position.y - mainCamera.ViewportToWorldPoint(new Vector3(viewportPos.x, lowerThresholdY, viewportPos.z)).y;
                newCamPos.y += deltaY;
            }

            transform.position = newCamPos;
            lastCameraX = transform.position.x;
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
}
