using UnityEngine;

public class Enemy_Jumper : Enemy
{
    private enum JumperState
    {
        Idle,
        Chase,
    }

    [Header("Ranges")]
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float walkingRange = 8f;

    [Header("Jump Forces")]
    [SerializeField] private float jumpForceX = 6f;
    [SerializeField] private float jumpForceY = 8f;


    [SerializeField] private JumperAnimator animator;

    private JumperEnemiesManager manager;
    private JumperState state = JumperState.Idle;
    private bool hasAttacked = false;

    protected override void Start()
    {
        base.Start();
        manager = GameManager.Instance.GetJumperEnemiesManager();
        manager.Register(this);

        animator.SetFalling();
    }

    private void OnDestroy()
    {
        manager?.Unregister(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isPaused) return;
        if (!isGrounded) return;

        switch (state)
        {
            case JumperState.Idle:
                {
                    break;
                }
            case JumperState.Chase:
                {
                    Chase();
                    break;
                }
        }
    }

    protected override void CheckGrounded()
    {
        isGrounded = false;
        if (rb.linearVelocityY > 0) return;

        float extraHeight = 0.1f;

        Debug.DrawRay(transform.position, Vector2.down * (rayLength + extraHeight), Color.green);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength + extraHeight);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Cage"))
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

    public void RetreatJump()
    {
        int randomSide = Random.Range(0, 2);
        Transform destinationPoint = GameManager.Instance
            .GetSideScrollCamera()
            .GetJumperLateralPoints()[randomSide];

        Vector2 from = transform.position;
        Vector2 to = destinationPoint.position;

        float dirX = Mathf.Sign(to.x - from.x);

        float distX = Mathf.Abs(to.x - from.x);
        float verticalForce = Random.Range(6f, 10f) * 3f;
        float horizontalForce = Mathf.Clamp(distX * 1.5f, 6f, 14f) * 2f;

        jumpManager.JumpWithForce(dirX * horizontalForce, verticalForce);
        jumpManager.jumpPressed = false;

        animator.SetIsJumping();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        manager?.Unregister(this);
        animator.AnimateDeath();
    }

    protected override void Attack()
    {
        if (hasAttacked || state == JumperState.Idle) return;

        if (playerController.CurrentHealth() <= 0) return;

        hasAttacked = true;
        animator.SetAttacking(true);
        playerController.TakeDamage(1);
        state = JumperState.Idle;
        manager.NotifyAttack(this);
    }

    public void StartChase()
    {
        state = JumperState.Chase;
        hasAttacked = false;
    }

    private void Chase()
    {
        float distance = Vector2.Distance(transform.position, manager.GetPlayerTransform().position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= walkingRange)
        {
            WalkToPlayer();
        }
        else
        {
            JumpToPlayer();
        }

        spriteDirection = rb.linearVelocity.x >= 0 ? 1f : -1f;
    }

    private void WalkToPlayer()
    {
        animator.SetWalking(true);

        float dir = Mathf.Sign(manager.GetPlayerTransform().position.x - transform.position.x);
        spriteDirection = dir;

        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }

    private void JumpToPlayer()
    {
        if (!isGrounded) return;

        animator.SetIsJumping();

        Vector2 from = transform.position;
        Vector2 target = manager.GetPlayerTransform().position;

        float randomOffset = Random.Range(-3f, 3f);
        target.x += randomOffset;

        float dirX = Mathf.Sign(target.x - from.x);

        float distX = Mathf.Abs(target.x - from.x);

        float forceX = Mathf.Lerp(3f, jumpForceX, distX / walkingRange);

        float forceY = jumpForceY + Random.Range(-2f, 1f);

        jumpManager.JumpWithForce(dirX * forceX, forceY);
    }
}
