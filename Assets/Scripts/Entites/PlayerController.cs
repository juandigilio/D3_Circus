using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MyEntity
{
    [SerializeField] private GameObject sight;
    [SerializeField] private float sightOffset = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int maxLives = 8;
    [SerializeField] private float aimSmoothSpeed = 10f;
    [SerializeField] private bool combinatedInput = true;

    //1-Pistol 2-Automatic 3-Rifle
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private Camera mainCamera;
    private Vector2 inputDirection;
    private Vector2 aimDirection;
    private int currentWeapon = 0;
    private bool isShooting = false;
    private Vector3 originalScale;
    private Vector3 invertedScale;
    private float lastQuantizedAngle = 0f;
    private float smoothedAngle = 0f;



    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    protected override void Start()
    {
        base.Start();

        mainCamera = Camera.main;

        availableLives = maxLives;

        weapons[0].gameObject.SetActive(true);
        weapons[1].gameObject.SetActive(false);
        weapons[2].gameObject.SetActive(false);

        originalScale = weapons[0].transform.localScale;
        invertedScale = new Vector3(-originalScale.x, -originalScale.y, originalScale.z);

        SideScrollCamera sideScrollCamera = GameManager.Instance.GetSideScrollCamera();
        sideScrollCamera.SetPlayerTransform(transform);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
        Aim();
        Shoot();
    }

    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
    }

    public void Jump()
    {
        if (!doubleJumped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (!jumped)
            {
                jumped = true;
            }
            else
            {
                doubleJumped = true;
            }
        }
    }

    public void NextWeapon()
    {
        currentWeapon++;
        if (currentWeapon > 2) currentWeapon = 0;
        SwitchWeapon(NextWeapon);
    }

    public void PreviousWeapon()
    {
        currentWeapon--;
        if (currentWeapon < 0) currentWeapon = 2;
        SwitchWeapon(PreviousWeapon);
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection.x != 0)
        {
            direction = inputDirection.x;


            UpdateWeaponDirection();
        }
    }

    private void Shoot()
    {
        if (isShooting)
        {
            if (!weapons[currentWeapon].HasAmmo())
            {
                NextWeapon();
            }

            Vector2 shootDirection = (sight.transform.position - weapons[currentWeapon].GetFirePointWorldPos()).normalized;

            weapons[currentWeapon].Shoot(shootDirection);
        }
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

    public void SetAimDirection(Vector2 direction)
    {
        aimDirection = direction;
    }

    private void CheckScreenLimits()
    {
        float leftWorldX = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, mainCamera.nearClipPlane)).x;

        if (transform.position.x < leftWorldX)
        {
            transform.position = new Vector3(leftWorldX, transform.position.y, transform.position.z);
        }
    }

    private void Aim()
    {
        float angle;
        Vector2 newDirection;

        sight.transform.position = weapons[currentWeapon].GetFirePointWorldPos();

        if (combinatedInput)
        {
            newDirection = inputDirection;          
        }
        else
        {
            newDirection = aimDirection;
        }

        if (newDirection != Vector2.zero)
        {
            float rawAngle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            angle = Mathf.Round(rawAngle / 45f) * 45f;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 quantizedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            quantizedDirection *= sightOffset;

            weapons[currentWeapon].AimAt(angle);
            sight.transform.position += new Vector3(quantizedDirection.x, quantizedDirection.y, 0);
        }
        else
        {
            if (direction >= 0) angle = 0;
            else angle = 180;

            weapons[currentWeapon].AimAt(angle);
            sight.transform.position += new Vector3(direction * sightOffset, 0, 0);
        }


    }

    private void SwitchWeapon(Action onNoAmmo)
    {
        if (weapons[currentWeapon].HasAmmo())
        {
            weapons[currentWeapon].gameObject.SetActive(true);
            UpdateWeaponDirection();

            for (int i = 0; i < weapons.Count; i++)
            {
                if (i != currentWeapon)
                {
                    weapons[i].gameObject.SetActive(false);
                }
            }
            return;
        }
        else
        {
            onNoAmmo?.Invoke();
        }
    }

    private void UpdateWeaponDirection()
    {
        if (direction > 0)
        {
            weapons[currentWeapon].transform.localScale = originalScale;
        }
        else if (direction < 0)
        {
            weapons[currentWeapon].transform.localScale = invertedScale;
        }
    }

    public void HealthUp()
    {
        availableLives++;

        if (availableLives > maxLives)
        {
            availableLives = maxLives;
        }
    }

    public int CurrentWeaponAmmo()
    {
        return weapons[currentWeapon].GetCurrentAmmo();
    }

    public int AvailableLives()
    {
        return availableLives;
    }

    public int MaxLives()
    {
        return maxLives;
    }
}