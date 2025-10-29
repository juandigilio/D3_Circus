using UnityEngine;
using System;


public class PlayerController : MyEntity
{
    [SerializeField] private int maxHealth = 8;
    [SerializeField] private WeaponsManager weaponsManager;
    [SerializeField] private AimController aimController;
    [SerializeField] private Transform startPos;
    [SerializeField] private PlayerAnimator animator;


    public static event Action OnPlayerDied;


    private CharacterAudio characterAudio;
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


        SideScrollCamera sideScrollCamera = GameManager.Instance.GetSideScrollCamera();
        sideScrollCamera.SetPlayerTransform(transform);

        characterAudio = GetComponent<CharacterAudio>();

        health = maxHealth;
        transform.position = startPos.position;

        GameManager.Instance.GetSideScrollCamera().RestartCamera();
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

            animator.SetRunning(true);
            animator.SetWeaponDirection(SetAnimatorDirection());
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetRunning(false);
            animator.SetWeaponDirection(Vector2.zero);
        }

        CheckScreenLimits();
    }

    private Vector2 SetAnimatorDirection()
    {
        Vector2 animatorDirection = Vector2.zero;

        if (inputDirection.x != 0)
        {
            animatorDirection.x = 1;
        }

        if (inputDirection.y > 0)
        {
            animatorDirection.y = 1;
        }
        else if (inputDirection.y < 0)
        {
            animatorDirection.y = -1;
        }

        return animatorDirection;
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

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        //characterAudio.PlayHitSound();

        if (health <= 0)
        {
            KillPlayer();
        }
    }

    private async void KillPlayer()
    {
        OnPlayerDied?.Invoke();
    }
}