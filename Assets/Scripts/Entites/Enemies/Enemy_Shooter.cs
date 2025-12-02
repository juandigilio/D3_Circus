using UnityEngine;

public class Enemy_Shooter : Enemy
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private ShooterAnimator animator;
    [SerializeField] private LayerMask raycastMask;


    private ShooterEnemySpawner spawner;
    private float shootDistance;


    private static readonly float[] attackAngles = { 45f, 0f, -45f };


    protected override void Start()
    {
        base.Start();

        health = 1;
        shootDistance = weapon.GetWeaponRange();

        Boss.OnBossDied += TakeDamage;
    }

    private void OnDestroy()
    {
        Boss.OnBossDied -= TakeDamage;
    }

    private void Update()
    {
        if (isPaused) return;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isPaused) return;
        if (isDead) return;

        if (!animator.IsShooting())
        {
            Patroll();
            Attack();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetSpawner(ShooterEnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void SetPatrollPoints(Transform pointA, Transform pointB)
    {
        leftPoint = pointA;
        rightPoint = pointB;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (health <= 0)
        {
            if (spawner != null)
                spawner.Unregister(this);

            isDead = true;
            animator.AnimateDeath();
        }
    }

    private void TakeDamage()
    {
        base.TakeDamage(9999);

        if (health <= 0)
        {
            if (spawner != null)
                spawner.Unregister(this);

            isDead = true;
            animator.AnimateDeath();
        }
    }

    private void Shoot(Vector2 direction, Vector2 newFirePoint, float angle)
    {
        weapon.Shoot(direction, newFirePoint, angle);
    }

    protected override void Attack()
    {
        if (playerController.CurrentHealth() <= 0) return;
        if (isPaused) return;

        bool movingRight = spriteDirection > 0;
        isAttacking = false;

        if ((movingRight && playerController.transform.position.x < transform.position.x) ||
            (!movingRight && playerController.transform.position.x > transform.position.x))
        {

            return;
        }

        int index = 0;

        foreach (float angle in attackAngles)
        {
            Vector2 baseDir = new Vector2(spriteDirection, 0f);

            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * baseDir;

            Vector2 firePoint = new Vector2(); ;

            if (spriteDirection > 0)
            {
                firePoint = animator.GetFirePoints()[index].position;
            }
            else
            {
                firePoint = animator.GetFirePoints()[2 - index].position;
            }

            index++;

            RaycastHit2D hit = Physics2D.Raycast(firePoint, dir, shootDistance, raycastMask);
            Debug.DrawRay(firePoint, dir * shootDistance, Color.yellow, 0.1f);

            if (hit.collider == null)
                continue;

            if (hit.collider.CompareTag("Bullet"))
                continue;

            if (hit.collider.CompareTag("Player"))
            {
                Shoot(dir, firePoint, angle);
                isAttacking = true;

                if (spriteDirection > 0)
                    animator.AnimateShoot(index - 1);
                else
                    animator.AnimateShoot(3 - index);

                break;
            }
        }
    }
}
