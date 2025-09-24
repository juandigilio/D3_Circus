using UnityEngine;

public abstract class MyEntity : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected int availableLives;
    [SerializeField] private float rayLength;

    protected Rigidbody2D rb;
    protected float direction = 1f;
    protected bool jumped = false;
    protected bool doubleJumped = false;



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
    }

    protected virtual void FixedUpdate()
    {
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
                jumped = false;
                doubleJumped = false;
                break;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        availableLives -= damage;

        if (availableLives <= 0)
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
}
