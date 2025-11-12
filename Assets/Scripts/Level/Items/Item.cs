using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected PlayerController playerController;

    [Header("Outline Effect")]
    [SerializeField] private SpriteRenderer outlineRenderer;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 1f;


    private float timer = 0;
    private float alpha = 1;
    protected virtual void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
    }

    protected virtual void FixedUpdate()
    {
        AnimateOutline();
    }

    private void AnimateOutline()
    {
        if (outlineRenderer == null)
        {
            Debug.Log("null render");
            return;
        }

        //timer += Time.fixedDeltaTime;

        //if (timer >= pulseSpeed)
        //{
        //    timer = 0;

        //    if (alpha == minAlpha)
        //    {
        //        alpha = maxAlpha;
        //    }
        //    else
        //    {
        //        alpha = minAlpha;
        //    }

        //    Color c = outlineRenderer.color;
        //    c.a = alpha;
        //    outlineRenderer.color = c;
        //}
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        Color c = outlineRenderer.color;
        c.a = alpha;
        outlineRenderer.color = c;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        if (collision.CompareTag("Player"))
        {
            PickUp();
            Destroy(gameObject);
        }
    }

    protected abstract void PickUp();
}
