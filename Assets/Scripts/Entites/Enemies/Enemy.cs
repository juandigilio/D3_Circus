using UnityEngine;

public abstract class Enemy : MyEntity
{
    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected int damage = 1;

    protected PlayerController playerController;
    protected EnemyAudio enemyAudio;
    protected bool isAttacking = false;


    protected override void Start()
    {
        base.Start();

        playerController = GameManager.Instance.GetPlayerController();
        enemyAudio = GetComponent<EnemyAudio>();

        if (leftPoint != null && rightPoint != null)
        {      
            transform.position = leftPoint.position;
        }       
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    protected void Patroll()
    {
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (direction > 0)
        {
            if (transform.position.x >= rightPoint.position.x) direction = -1;
        }
        else
        {
            if (transform.position.x <= leftPoint.position.x) direction = 1;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    protected abstract void Attack();

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        //enemyAudio.PlayHitSound();
    }
}

