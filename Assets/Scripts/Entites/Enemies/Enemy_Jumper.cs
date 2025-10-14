using UnityEngine;

public class Enemy_Jumper : Enemy
{
    [SerializeField] private float walkingRange = 3f;  
    [SerializeField] private float jumpingRange = 7f;  
    [SerializeField] private float maxJumpHeight = 4f; 
    [SerializeField] private float minJumpHeight = 2f; 
    [SerializeField] private float attackRange = 0.8f; 
    [SerializeField] private float attackCooldown = 1f;

    [SerializeField] private LayerMask groundMask;

    protected override void Start()
    {
        base.Start();

        availableLives = 1;
    }

    protected override void FixedUpdate()
    {
        if (!isPaused)
        {
            base.FixedUpdate();

            UpdateCurrentState();
        }  
    }

    private void UpdateCurrentState()
    {
        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (isGrounded)
        {
            if (transform.position.x < playerController.transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            if (distance <= walkingRange)
            {
                WalkTowardsPlayer();
            }
            else if (distance <= jumpingRange)
            {
                JumpTowardsPlayer();
            }
        }

        UpdateAssetDirection();

        CheckAttackState(distance);
    }

    private void CheckAttackState(float distance)
    {
        if (distance <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                InvokeRepeating(nameof(Attack), 0f, attackCooldown);
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                CancelInvoke(nameof(Attack));
            }
        }
    }

    private void WalkTowardsPlayer()
    {
        float dir = Mathf.Sign(playerController.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
    }

    private void JumpTowardsPlayer()
    {
        Vector2 dir = playerController.transform.position - transform.position;
        float dirX = Mathf.Sign(dir.x);
        float jumpForceY = Mathf.Clamp(dir.y + 2f, minJumpHeight, maxJumpHeight);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, jumpForceY, groundMask);
        if (hit.collider != null)
        {
            jumpForceY = hit.distance * 0.9f;
        }

        rb.linearVelocity = new Vector2(dirX * speed * 1.2f, jumpForceY);
    }

    protected override void Attack()
    {
        playerController.TakeDamage(damage);
        Debug.Log("Enemy_Jumper attacked the player!");
    }
}
