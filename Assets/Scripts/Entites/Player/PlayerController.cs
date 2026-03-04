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
    private bool isCrauching = false;
    private bool isBossDead = false;

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    protected override void Start()
    {
        base.Start();

        mainCamera = Camera.main;


        SideScrollCamera sideScrollCamera = GameManager.Instance.GetSideScrollCamera();

        characterAudio = GetComponent<CharacterAudio>();

        health = maxHealth;
        transform.position = startPos.position;

        GameManager.Instance.GetSideScrollCamera().RestartCamera();

        Boss.OnBossDied += SetPaused;
        Boss.OnBossDied += SetBossDead;
    }

    private void OnDestroy()
    {
        Boss.OnBossDied -= SetPaused;
        Boss.OnBossDied -= SetBossDead;
    }

    protected override void FixedUpdate()
    {
        if (isBossDead)
        {
            rb.linearVelocity = Vector2.zero;
        }

        base.FixedUpdate();

        if (!isPaused)
        {
            Move();
            SetAimControllerDirection();
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection.x != 0)
        {
            spriteDirection = inputDirection.x;
        }

        aimController.SetInputDirection(inputDirection);
    }

    public void SetAimDirection(Vector2 aimDirection)
    {
        aimController.SetAimDirection(aimDirection);
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

    public int CurrentWeaponAmmo()
    {
        return weaponsManager.CurrentWeaponAmmo();
    }

    public WeaponType GetCurrentWeaponType()
    {
        return weaponsManager.GetCurrentWeaponType();
    }

    public float CurrentHealth()
    {
        return health;
    }

    public int MaxHealth()
    {
        return maxHealth;
    }

    public void HealthUp(int healthAmount)
    {
        health += healthAmount;

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
        spriteDirection = direction;
    }

    public override void TakeDamage(float damage)
    {
        if (isBossDead) return;
        if (health <= 0) return;

        base.TakeDamage(damage);

        if (health <= 0)
        {
            KillPlayer();
        }
        else
        {
            characterAudio.PlayHitSound();
        }
    }

    public void AddAmmo(WeaponType weaponType, int ammoAmount)
    {
        weaponsManager.AddAmmo(weaponType, ammoAmount);
    }

    public WeaponsManager GetWeaponsManager()
    {
        return weaponsManager;
    }

    public PlayerAnimator GetPlayerAnimator()
    {
        return animator;
    }

    public void FinishGame()
    {
        OnPlayerDied?.Invoke();
    }

    public bool IsCrouching()
    {
        return isCrauching;
    }

    public void SetCrouching(bool crouching)
    {
        isCrauching = crouching;
    }

    private void KillPlayer()
    {
        GameManager.Instance.GetMusicController().SetDeathState();
        characterAudio.PlayDeathSound();
        GameManager.Instance.GetLevelManager().HideAll();
        animator.ShowDeath();
    }

    private void CheckScreenLimits()
    {
        float leftWorldX = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, mainCamera.nearClipPlane)).x;
        float rightWorldX = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, mainCamera.nearClipPlane)).x;

        Vector3 pos = transform.position;

        if (pos.x < leftWorldX)
            pos.x = leftWorldX;

        if (pos.x > rightWorldX)
            pos.x = rightWorldX;

        transform.position = pos;
    }

    private void SetBossDead()
    {
        isBossDead = true;
        animator.RestartPosition();
    }

    private void SetAimControllerDirection()
    {
        aimController.SetDirection(spriteDirection);
    }

    private void Move()
    {
        if (inputDirection.x != 0)
        {
            Vector2 movement = new Vector2(inputDirection.x, rb.linearVelocity.y);

            if (movement.x > 0)
            {
                movement.x = 1 * speed;
            }
            else if (movement.x < 0)
            {
                movement.x = -1 * speed;
            }

            rb.linearVelocity = movement;

            animator.SetRunning(true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetRunning(false);
        }

        CheckScreenLimits();
    }

}