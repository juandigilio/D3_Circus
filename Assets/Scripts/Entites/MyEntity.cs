using UnityEngine;

public abstract class MyEntity : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected float health;
    [SerializeField] protected float rayLength;
    [SerializeField] protected JumpManager jumpManager;
    [SerializeField] private Collider2D entityCollider;
    [SerializeField] private float deathYThreshold = -10f;

    protected Rigidbody2D rb;
    protected float spriteDirection = 1f;
    protected bool isPaused = false;
    protected bool isDead = false;
    private bool isFrozen = false;


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
        if (isDead)
        {
            if (transform.position.y < deathYThreshold)
            {
                Vector2 position = transform.position;
                position.y = deathYThreshold;

                transform.position = position;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        if (isPaused) return;

        CheckGrounded();

        UpdateAssetDirection();
    }

    protected virtual void CheckGrounded()
    {
        isGrounded = false;
        if (rb.linearVelocityY > 0) return;

        float extraHeight = 0.1f;

        Debug.DrawRay(transform.position, Vector2.down * (rayLength + extraHeight), Color.green);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength + extraHeight);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Cage") || hit.collider.CompareTag("Platform"))
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

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            isPaused = true;

            if (entityCollider)
            {
                entityCollider.enabled = false;
            }
            
            Debug.Log($"{gameObject.name} died.");
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    protected virtual void UpdateAssetDirection()
    {
        if (spriteDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (spriteDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void SetPaused()
    {
        isPaused = true;
        if (rb)
        {
            rb.Sleep();
        }    
    }

    private void StopPause()
    {
        isPaused = false;
        if (rb)
        {
            rb.WakeUp();
        }        
    }
}
