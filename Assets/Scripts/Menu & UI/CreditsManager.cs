using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private GameObject creditsObj;

    [SerializeField] private float scrollSpeed = 100f;
    [SerializeField] private float startY = -600f;    
    [SerializeField] private float endY = 600f;       

    private RectTransform credits;
    private bool scrolling = false;

    private void Start()
    {
        credits = creditsObj.GetComponent<RectTransform>();

        Vector2 startPos = new Vector2(0, startY);
        credits.anchoredPosition = startPos;

        credits.anchorMin = new Vector2(0.5f, 0);
        credits.anchorMax = new Vector2(0.5f, 0);
        credits.pivot = new Vector2(0.5f, 0.5f);

        StartCoroutine(ShowCredits());
    }

    private IEnumerator ShowCredits()
    {
        scrolling = true;

        while (scrolling)
        {
            credits.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (credits.anchoredPosition.y >= endY)
            {
                scrolling = false;
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        LoadMainMenu();
    }

    private async void LoadMainMenu()
    {
        await SceneManager.GoBackToMenuAsync();
    }
}
