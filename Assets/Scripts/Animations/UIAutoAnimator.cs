using UnityEngine;
using UnityEngine.UI;


public class UIAutoAnimator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 10f;
    [SerializeField] private bool randomStartFrame = false;

    private int currentFrame;
    private float timer;

    private void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning($"{name}: no hay sprityes");
            enabled = false;
            return;
        }

        if (randomStartFrame)
            currentFrame = Random.Range(0, animationFrames.Length);

        image.sprite = animationFrames[currentFrame];
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            image.sprite = animationFrames[currentFrame];
        }
    }
}
