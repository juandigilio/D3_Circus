using UnityEngine;

public abstract class Enemy : MyEntity
{
    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int damage = 1;

    protected PlayerController playerController;
    protected bool movingRight = true;
    protected bool isAttacking = false;


    protected override void Start()
    {
        base.Start();

        playerController = GameManager.Instance.GetPlayerController();
        transform.position = leftPoint.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void Patrol()
    {
        Vector2 dir;
        if (movingRight)
        {
            dir = Vector2.right;
            if (transform.position.x >= rightPoint.position.x) movingRight = false;
        }
        else
        {
            dir = Vector2.left;
            if (transform.position.x <= leftPoint.position.x) movingRight = true;
        }

        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
    }

    protected abstract void Attack();
}

