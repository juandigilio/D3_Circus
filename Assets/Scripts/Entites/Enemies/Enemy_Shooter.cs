using UnityEngine;

public class Enemy_Shooter : Enemy
{
    [SerializeField] private Weapon weapon;

    private float shootDistance;


    protected override void Start()
    {
        base.Start();

        availableLives = 1;
        shootDistance = weapon.GetWeaponRange();
    }

    private void Update()
    {
        if (!isPaused)
        {

        }
    }

    protected override void FixedUpdate()
    {
        if (!isPaused)
        {
            base.FixedUpdate();

            Patroll();
            Attack();
        }    
    }

    private void Shoot(Vector2 direction)
    {
        weapon.Shoot(direction);
    }

    protected override void Attack()
    {
        bool movingRight = direction > 0;

        if ((movingRight && playerController.transform.position.x > transform.position.x) || 
            (!movingRight && playerController.transform.position.x < transform.position.x))
        {
            Vector2 direction = (playerController.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerController.transform.position);

            if (distance > shootDistance) 
            {
                isAttacking = false;
                return;
            }

            Debug.DrawRay(weapon.GetFirePointWorldPos(), direction * distance, Color.red, 0.1f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.GetFirePointWorldPos(), direction, distance);

            isAttacking = false;

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
                    isAttacking = true;
                }
                break;
            }
        }
        else
        {
            Debug.Log("No estoy mirando al player");
            isAttacking = false;
        }
    }
}
