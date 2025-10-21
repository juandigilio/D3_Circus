using UnityEngine;

public class JumpManager : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 3.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private bool jumped = false;
    private bool doubleJumped = false;
    [SerializeField] private bool jumpPressed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpPressed = false;
    }

    private void FixedUpdate()
    {
        ApplyBetterJumpPhysics();
    }

    public void Jump()
    {
        if (!doubleJumped)
        {
            jumpPressed = true;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (!jumped)
            {
                jumped = true;
            }
            else
            {
                doubleJumped = true;
            }
        }
    }

    public void JumpWithForce(float horizontalForce, float verticalForce)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
        jumpPressed = true;
    }

    private void ApplyBetterJumpPhysics()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocityY > 0 && !jumpPressed)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    public void ResetJumps()
    {
        jumped = false;
        doubleJumped = false;
    }

    public void StopJump()
    {
        jumpPressed = false;
    }
}
