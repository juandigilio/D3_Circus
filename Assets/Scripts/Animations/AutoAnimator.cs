using System;
using System.Collections;
using UnityEngine;

public class AutoAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 4f;
    [SerializeField] private bool isPausable = false;

    [Header("Breath + Wiggle")]
    [SerializeField] private bool breath = false;
    [SerializeField] private float horizontalAmplitude = 0.15f;
    [SerializeField] private float pulseAmplitude = 0.15f;
    [SerializeField] private float pulseSpeed = 2f;

    private int currentFrame;
    private float timer;
    private Vector3 startPos;
    private Vector3 startScale;
    private bool isPaused = false;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning($"{name}: No hay frames asignados al AutoAnimator.");
            enabled = false;
            return;
        }

        spriteRenderer.sprite = animationFrames[currentFrame];

        startPos = transform.localPosition;
        startScale = transform.localScale;

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
        MenuController.OnGameStarted += StopPause;
    }

    private void OnDestroy()
    {
        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
        MenuController.OnGameStarted -= StopPause;
    }

    private void Update()
    {
        if (isPausable && isPaused) return;

        AnimateFrames();
        BreathAndWiggle();
    }

    public void TurnOff(float duration = 0.5f, int flashes = 4)
    {
        StartCoroutine(TurnOffCoroutine(duration, flashes));
    }

    private IEnumerator TurnOffCoroutine(float duration, int flashes)
    {
        float flashTime = duration / (flashes * 2f);
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashes; i++)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime / flashTime)
            {
                Color c = originalColor;
                c.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = c;
                yield return null;
            }

            for (float t = 0; t < 1f; t += Time.deltaTime / flashTime)
            {
                Color c = originalColor;
                c.a = Mathf.Lerp(0f, 1f, t);
                spriteRenderer.color = c;
                yield return null;
            }
        }

        Color finalC = originalColor;
        finalC.a = 0f;
        spriteRenderer.color = finalC;

        gameObject.SetActive(false);
    }

    private void AnimateFrames()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
        }
    }

    private void BreathAndWiggle()
    {
        if (!breath) return;

        float t = Mathf.Sin(Time.time * pulseSpeed);

        float xOffset = t * horizontalAmplitude;
        transform.localPosition = startPos + new Vector3(xOffset, 0f, 0f);

        float scale = 1f + t * pulseAmplitude;
        transform.localScale = startScale * scale;
    }

    private void SetPaused()
    {
        isPaused = true;
    }

    private void StopPause()
    {
        isPaused = false;
    }
}
