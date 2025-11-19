using UnityEngine;

public class AutoAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 4f;

    [Header("Breath + Wiggle")]
    [SerializeField] private bool breath = false;
    [SerializeField] private float horizontalAmplitude = 0.15f;
    [SerializeField] private float pulseAmplitude = 0.15f;
    [SerializeField] private float pulseSpeed = 2f;

    private int currentFrame;
    private float timer;
    private Vector3 startPos;
    private Vector3 startScale;

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
    }

    private void Update()
    {
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
}
