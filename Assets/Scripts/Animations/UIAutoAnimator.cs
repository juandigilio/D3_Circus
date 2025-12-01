using UnityEngine;
using UnityEngine.UI;

public class UIAutoAnimator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 10f;
    [SerializeField] private bool randomStartFrame = false;

    [Header("Breath + Wiggle")]
    [SerializeField] private bool breath = false;
    [SerializeField] private float horizontalAmplitude = 6f;
    [SerializeField] private float pulseAmplitude = 0.1f;
    [SerializeField] private float pulseSpeed = 2f;

    [Header("Pause")]
    [SerializeField] private bool isPausable = false;

    private int currentFrame;
    private float timer;

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Vector3 startScale;

    private bool isPaused = false;


    private void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning($"{name}: No hay sprites en UIAutoAnimator.");
            enabled = false;
            return;
        }

        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;

        if (randomStartFrame)
            currentFrame = Random.Range(0, animationFrames.Length);

        image.sprite = animationFrames[currentFrame];

        PauseHandler.OnGamePaused += Pause;
        PauseHandler.OnGameContinue += Unpause;
        CutSceneManager.OnGameStarted += Unpause;
    }

    private void OnDestroy()
    {
        PauseHandler.OnGamePaused -= Pause;
        PauseHandler.OnGameContinue -= Unpause;
        CutSceneManager.OnGameStarted -= Unpause;
    }

    private void Update()
    {
        if (isPausable && isPaused)
            return;

        AnimateFrames();
        BreathAndWiggle();
    }

    private void AnimateFrames()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            image.sprite = animationFrames[currentFrame];
        }
    }

    private void BreathAndWiggle()
    {
        if (!breath) return;

        float t = Mathf.Sin(Time.time * pulseSpeed);

        float xOffset = t * horizontalAmplitude;
        rectTransform.anchoredPosition = startPos + new Vector2(xOffset, 0f);

        float scale = 1f + t * pulseAmplitude;
        rectTransform.localScale = startScale * scale;
    }

    private void Pause()
    {
        isPaused = true;
    }

    private void Unpause()
    {
        isPaused = false;
    }
}
