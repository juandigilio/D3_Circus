using UnityEngine;

public class Enemy_Jumper : Enemy
{
    [Header("Ranges")]
    [SerializeField] private float walkingRange = 3f;
    [SerializeField] private float attackRange = 0.8f;
    [SerializeField] private float retreatDistance = 4f;
    [SerializeField] private float jumpingRange = 7f;

    [Header("Jump Forces")]
    [SerializeField] private float jumpForceBase = 10f;
    [SerializeField] private float retractForce = 10f;
    [SerializeField] private float horizontalForceMultiplier = 0.8f;
    [SerializeField] private float maxJumpHeight = 4f;
    [SerializeField] private float checkStep = 0.5f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Behavior Timing")]
    [SerializeField] private float jumpCooldown = 0.6f;
    private float nextJumpTime = 0f;

    private bool retreating = false;

    protected override void FixedUpdate()
    {
        if (isPaused) return;
        base.FixedUpdate();

        if (!isGrounded) return;

        if (retreating)
        {
            RetreatJump();
            return;
        }

        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (distance <= walkingRange && distance > attackRange)
        {
            WalkTowardsPlayer();
        }
        else if (distance > attackRange)
        {
            JumpTowardsPlayer();
        }
        else
        {
            Attack();
        }
    }

    private void WalkTowardsPlayer()
    {
        float horizontalDir = Mathf.Sign(playerController.transform.position.x - transform.position.x);
        direction = horizontalDir;
        rb.linearVelocity = new Vector2(horizontalDir * speed, rb.linearVelocity.y);
    }

    private void JumpTowardsPlayer()
    {
        Vector2 playerPos = playerController.transform.position;
        Vector2 from = transform.position;
        float horizontalDir = Mathf.Sign(playerPos.x - from.x);
        direction = horizontalDir;
        float distance = Mathf.Abs(playerPos.x - from.x);

        float minVertical = 3f;
        float verticalForce = Mathf.Lerp(minVertical, maxJumpHeight, distance / jumpingRange);
        float horizontalForce = horizontalDir * Mathf.Lerp(2f, 6f, distance / jumpingRange);

        for (float y = 0f; y < maxJumpHeight; y += checkStep)
        {
            Vector2 origin = from + new Vector2(0, y);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, checkStep, obstacleMask);
            if (hit.collider != null)
            {
                verticalForce = Mathf.Max(2f, y * 0.8f);
                break;
            }
        }

        jumpManager.JumpWithForce(horizontalForce, verticalForce);
    }

    private void RetreatJump()
    {
        float horizontalDir = -Mathf.Sign(playerController.transform.position.x - transform.position.x);
        direction = horizontalDir;
        float horizontalForce = horizontalDir * retractForce;
        float verticalForce = retractForce * 0.8f;

        jumpManager.JumpWithForce(horizontalForce, verticalForce);
        retreating = false;
    }

    protected override void Attack()
    {
        playerController.TakeDamage(damage);
        retreating = true;
    }
}
