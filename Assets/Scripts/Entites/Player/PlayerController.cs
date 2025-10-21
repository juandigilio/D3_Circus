using UnityEngine;


public class PlayerController : MyEntity
{
    [SerializeField] private int maxHealth = 8;
    [SerializeField] private WeaponsManager weaponsManager;
    [SerializeField] private AimController aimController;

    private Camera mainCamera;
    private Vector2 inputDirection;

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    protected override void Start()
    {
        base.Start();

        mainCamera = Camera.main;

        health = maxHealth;

        SideScrollCamera sideScrollCamera = GameManager.Instance.GetSideScrollCamera();
        sideScrollCamera.SetPlayerTransform(transform);
    }

    protected override void FixedUpdate()
    {
        if (!isPaused)
        {
            base.FixedUpdate();

            Move();
            SetAimControllerDirection();
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection.x != 0)
        {
            direction = inputDirection.x;
        }

        aimController.SetInputDirection(inputDirection);
    }

    public void SetAimDirection(Vector2 aimDirection)
    {
        aimController.SetAimDirection(aimDirection);
    }

    private void SetAimControllerDirection()
    {
        aimController.SetDirection(direction);
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            Vector2 movement = new Vector2(inputDirection.x * speed, rb.linearVelocity.y);
            rb.linearVelocity = movement;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        CheckScreenLimits();
    }

    public void Jump()
    {
        if (isPaused) return;

        jumpManager.Jump();
    }

    public void StopJump()
    {
        jumpManager.StopJump();
    }

    public void SetShooting(bool shooting)
    {
        weaponsManager.SetShooting(shooting);
    }

    public void NextWeapon()
    {
        weaponsManager.NextWeapon();
    }

    public void PreviousWeapon()
    {
        weaponsManager.PreviousWeapon();
    }

    private void CheckScreenLimits()
    {
        float leftWorldX = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, mainCamera.nearClipPlane)).x;

        if (transform.position.x < leftWorldX)
        {
            transform.position = new Vector3(leftWorldX, transform.position.y, transform.position.z);
        }
    }

    public int CurrentWeaponAmmo()
    {
        return weaponsManager.CurrentWeaponAmmo();
    }

    public WeaponType GetCurrentWeaponType()
    {
        return weaponsManager.GetCurrentWeaponType();
    }

    public int AvailableLives()
    {
        return health;
    }

    public int MaxLives()
    {
        return maxHealth;
    }

    public void HealthUp()
    {
        health++;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float HealthPercentage()
    {
        return (float)health / (float)maxHealth;
    }

    public void SetDirection(float direction)
    {
        this.direction = direction;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}