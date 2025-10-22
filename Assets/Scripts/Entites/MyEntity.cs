using UnityEngine;

public abstract class MyEntity : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected int health;
    [SerializeField] private float rayLength;
    [SerializeField] protected JumpManager jumpManager;

    protected Rigidbody2D rb;
    protected float direction = 1f;
    protected bool isPaused = false;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this gameobject");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D component missing from this gameobject");
        }

        rayLength = col.bounds.extents.y;

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

    protected virtual void FixedUpdate()
    {
        if (isPaused) return;

        CheckGrounded();

        UpdateAssetDirection();

    }

    protected void CheckGrounded()
    {
        isGrounded = false;
        if (rb.linearVelocityY > 0) return;

        float extraHeight = 0.1f;

        Debug.DrawRay(transform.position, Vector2.down * (rayLength + extraHeight), Color.green);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength + extraHeight);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;

                if (jumpManager)
                {
                    jumpManager.ResetJumps();
                }
                    
                break;
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected void UpdateAssetDirection()
    {
        if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void SetPaused()
    {
        isPaused = true;
        rb.Sleep();
    }

    private void StopPause()
    {
        isPaused = false;
        rb.WakeUp();
    }
}
