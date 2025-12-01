using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Image[] cutScene;
    [SerializeField] private GameObject[] conversation;
    [SerializeField] private float[] textTimer;
    [SerializeField] private float toneDuration;
    [SerializeField] private float image0Duration = 2f;
    [SerializeField] private float image1Duration = 1f;


    private float speachDuration = 0;
    private float fixedTimer;
    private bool toneFinished = false;
    private bool gameLoaded = false;
    private int conversationIndex = 0;


    public static event System.Action OnGameStarted;

    private void Start()
    {
        StartCoroutine(AnimateCutscene());

        foreach (float t in textTimer)
        {
            speachDuration += t;
        }
        speachDuration += toneDuration;
    }


    private void FixedUpdate()
    {
        fixedTimer += Time.fixedDeltaTime;

        if (fixedTimer >= toneDuration && !toneFinished)
        {
            toneFinished = true;
            StartCoroutine(AnimateConversation());
        }
        else if (fixedTimer >= speachDuration)
        {
            LoadGame();
        }
    }

    private IEnumerator AnimateCutscene()
    {
        while (!toneFinished)
        {
            cutScene[0].gameObject.SetActive(true);
            cutScene[1].gameObject.SetActive(false);
            yield return new WaitForSeconds(image0Duration);

            cutScene[0].gameObject.SetActive(false);
            cutScene[1].gameObject.SetActive(true);
            yield return new WaitForSeconds(image1Duration);
        }

        cutScene[0].gameObject.SetActive(true);
        cutScene[1].gameObject.SetActive(false);
    }

    private IEnumerator AnimateConversation()
    {
        while (conversationIndex < conversation.Length)
        {
            conversation[conversationIndex].SetActive(true);
            yield return new WaitForSeconds(textTimer[conversationIndex]);
            conversation[conversationIndex].SetActive(false);
            conversationIndex++;
        }
    }

    public async void LoadGame()
    {
        if (gameLoaded) return;

        gameLoaded = true;
        OnGameStarted?.Invoke();

        await SceneManager.LoadGameAsync();
    }
}
