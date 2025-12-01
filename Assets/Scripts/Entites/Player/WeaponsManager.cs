using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    [SerializeField] private GameObject sight;
    [SerializeField] private PlayerAnimator animator;

    private PlayerController playerController;
    private CharacterAudio characterAudio;
    private int currentWeapon = 0;
    private bool isShooting = false;
    private Transform currentFirePoint;

    private void OnEnable()
    {
        GameManager.Instance.RegisterWeaponsManager(this);
        currentFirePoint = transform;

    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterAudio = GetComponent<CharacterAudio>();

        weapons[0].gameObject.SetActive(true);
        weapons[1].gameObject.SetActive(false);
        weapons[2].gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playerController.IsPaused()) return;

        Shoot();
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
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

            if (weapons[currentWeapon].Shoot(shootDirection, angle))
            {
                characterAudio.PlayShootSound();
                animator.AnimateShoot();
            }
        }
    }

    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
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

    public int CurrentWeaponAmmo()
    {
        return weapons[currentWeapon].GetCurrentAmmo();
    }

    public WeaponType GetCurrentWeaponType()
    {
        return weapons[currentWeapon].GetWeaponType();
    }

    private void SwitchWeapon(Action onNoAmmo)
    {
        if (weapons[currentWeapon].HasAmmo())
        {
            weapons[currentWeapon].gameObject.SetActive(true);
            animator.SetWeapon(currentWeapon);

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

    public void AddAmmo(WeaponType weaponType, int ammoAmount)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.GetWeaponType() == weaponType)
            {
                weapon.AddAmmo(ammoAmount);
                return;
            }
        }
    }

    public void SetCurrentFirePoint(Transform firePoint)
    {
        currentFirePoint = firePoint;
    }

    public Transform GetCurrentFirePoint()
    {
        return currentFirePoint;
    }

    public int GetMachineGunAmmo()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.GetWeaponType() == WeaponType.Automatic)
            {
                return weapon.GetCurrentAmmo();
            }
        }
        return 0;
    }

    public int GetRifleAmmo()
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.GetWeaponType() == WeaponType.ShotGun)
            {
                return weapon.GetCurrentAmmo();
            }
        }
        return 0;
    }
}
