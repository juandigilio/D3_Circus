using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onFrame1;
    [SerializeField] private Sprite onFrame2;
    [SerializeField] private float animationSpeed = 0.2f;

    private UIAudio uiAudio;
    private Image image;
    private Coroutine animCoroutine;

    private Button button;

    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = offSprite;

        button = GetComponent<Button>();
    }

    private void Start()
    {
        uiAudio = GameManager.Instance.GetComponent<UIAudio>();
    }


    private void OnMouseExit()
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        image.sprite = offSprite;
    }

    public void AnimateButton()
    {
        animCoroutine = StartCoroutine(Animate());

        Debug.Log("pointer entered");
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

    public void PlayClickSound()
    {
        uiAudio.PlayClickSound();
        Debug.Log("click sound played");
    }

    public async void LoadCreditsScene()
    {
        await SceneManager.LoadCreditsScene();
    }

    public async void LoadMainMenuScene()
    {
        await SceneManager.GoBackToMenuAsync();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("mouse en botoncito");
        AnimateButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        image.sprite = offSprite;
    }
}
