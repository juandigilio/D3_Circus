using UnityEngine;

public class AutoAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 4f;
    [SerializeField] private bool randomStartFrame = false;

    private int currentFrame;
    private float timer;

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

        if (randomStartFrame)
            currentFrame = Random.Range(0, animationFrames.Length);

        spriteRenderer.sprite = animationFrames[currentFrame];
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
        }
    }
}
