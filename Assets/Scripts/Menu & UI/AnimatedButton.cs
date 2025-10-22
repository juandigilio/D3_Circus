using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedButton : MonoBehaviour
{
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onFrame1;
    [SerializeField] private Sprite onFrame2;
    [SerializeField] private float animationSpeed = 0.2f;

    private UIAudio uiAudio;
    private Image image;
    private Coroutine animCoroutine;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = offSprite;
    }

    private void Start()
    {
        uiAudio = GameManager.Instance.GetComponent<UIAudio>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animCoroutine = StartCoroutine(Animate());

        Debug.Log("pointer entered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        image.sprite = offSprite;
    }

    IEnumerator Animate()
    {
        while (true)
        {
            image.sprite = onFrame1;
            yield return new WaitForSeconds(animationSpeed);
            image.sprite = onFrame2;
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    public void PlayHoverSound()
    {
        //uiAudio.PlayHoverSound();
        Debug.Log("hover sound played");
    }

    public void StopHoverSound()
    {

    }

    public void PlayClickSound()
    {
        //uiAudio.PlayClickSound();
        Debug.Log("click sound played");
    }
}
