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

        int enemyLayer = LayerMask.NameToLayer("EnemyJumper");
        int platformLayer = LayerMask.NameToLayer("Platform");

        Physics2D.IgnoreLayerCollision(enemyLayer, platformLayer, true);
    }

    private void OnDestroy()
    {
        manager?.Unregister(this);
    }

    protected override void FixedUpdate()
    {
        if (isPaused) return;
        base.FixedUpdate();

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
        float verticalForce = Random.Range(6f, 10f) * 3f; // escala para parecerse al salto normal
        float horizontalForce = Mathf.Clamp(distX * 1.5f, 6f, 14f) * 2f;

        jumpManager.JumpWithForce(dirX * horizontalForce, verticalForce);
        jumpManager.jumpPressed = false;

        // Opcional: para evitar que choque con paredes mientras sube
        //rb.linearVelocity = new Vector2(dirX * horizontalForce, verticalForce);

        animator.SetIsJumping();
    }

    public override void TakeDamage(float damage)
    {
        manager?.Unregister(this);

        Destroy(gameObject);
    }

    protected override void Attack()
    {
        if (hasAttacked) return;

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

        float randomOffset = Random.Range(-2f, 2f); //podés ajustar este valor
        target.x += randomOffset;

        float dirX = Mathf.Sign(target.x - from.x);

        float distX = Mathf.Abs(target.x - from.x);

        float forceX = Mathf.Lerp(3f, jumpForceX, distX / walkingRange);

        float forceY = jumpForceY + Random.Range(-2f, 1f);

        jumpManager.JumpWithForce(dirX * forceX, forceY);
    }
}
