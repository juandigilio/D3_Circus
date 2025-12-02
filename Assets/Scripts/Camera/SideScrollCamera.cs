using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [SerializeField] GameObject continueCollider;
    [SerializeField] private float scrollDuration = 2f;
    [SerializeField] private List<Transform> baloonsSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> jumperLateralPoints = new List<Transform>();
    [SerializeField] private List<Transform> jumperUpperPoints = new List<Transform>();

    private Vector3 startPos;
    private Camera mainCamera;
    private bool isMoving = false;
    private bool isCameraLocked = true;

    private void Start()
    {
        mainCamera = Camera.main;
        GameManager.Instance.RegisterSideSrollCamera(this);

        startPos = mainCamera.transform.position;

        isCameraLocked = true;
        continueCollider.SetActive(false);
    }

    public void Continue()
    {
        continueCollider.SetActive(false);
        GameManager.Instance.GetContinueArrow().SetActive(false);

        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        isMoving = true;
        float screenWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
        float distance = screenWidth * 0.9f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(distance, 0f, 0f);
        
        float t = 0f;
        GameManager.Instance.GetLevelManager().LoadNextSector();

        while (t < 1f)
        {
            t += Time.deltaTime / scrollDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
        isCameraLocked = true;
    }

    public void Unlock()
    {
        if (!isCameraLocked) return;

        isCameraLocked = false;
        continueCollider.SetActive(true);
        GameManager.Instance.GetContinueArrow().SetActive(true);
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void RestartCamera()
    {
        transform.position = startPos;
    }

    public List<Transform> GetBaloonSpawnPoints()
    {
        return baloonsSpawnPoints;
    }

    public List<Transform> GetJumperLateralPoints()
    {
        return jumperLateralPoints;
    }

    public List<Transform> GetJumperUpperPoints()
    {
        return jumperUpperPoints;
    }
}
