using UnityEngine;

public class Enemy_Shooter : Enemy
{
    [SerializeField] private Weapon weapon;

    private bool isShooting = false;
    private float shootDistance;


    protected override void Start()
    {
        base.Start();

        availableLives = 1;
        shootDistance = weapon.GetWeaponRange();
    }

    private void Update()
    {
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Patroll();
        Attack();
    }

    private void Patroll()
    {
        if (isShooting)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        Vector2 direction;

        if (movingRight)
        {
            direction = Vector2.right;

            if (transform.position.x >= rightPoint.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            direction = Vector2.left;

            if (transform.position.x <= leftPoint.position.x)
            {
                movingRight = true;
            }
        }

        Vector2 movemenDirection = new Vector2(direction.x * speed, rb.linearVelocity.y);
        rb.linearVelocity = movemenDirection;
    }

    private void Shoot(Vector2 direction)
    {
        weapon.Shoot(direction);
    }

    protected override void Attack()
    {
        if ((movingRight && playerController.transform.position.x > transform.position.x) || 
            (!movingRight && playerController.transform.position.x < transform.position.x))
        {
            Vector2 direction = (playerController.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerController.transform.position);

            if (distance > shootDistance) 
            {
                isShooting = false;
                return;
            }

            Debug.DrawRay(weapon.GetFirePointWorldPos(), direction * distance, Color.red, 0.1f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.GetFirePointWorldPos(), direction, distance);

            isShooting = false;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                if (hit.collider.CompareTag("Bullet"))
                {
                    continue;
                }

                if (hit.collider.CompareTag("Player"))
                {
                    Shoot(direction);
                    isShooting = true;
                }
                //else
                //{
                //    isShooting = false;
                //}

                break;
            }
        }
        else
        {
            Debug.Log("No estoy mirando al player");
            isShooting = false;
        }
    }
}
