using UnityEngine;

public enum WeaponType
{
    Pistol,
    Automatic,
    Rifle,
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifeDistance;
    [SerializeField] int bulletDamage;
    [SerializeField] bool bulletIsDestroyable;

    [SerializeField] bool isPlayerWeapon;
    [SerializeField] private Transform firePoint;

    private float fireCooldown;
    private int currentAmmo;


    private void Awake()
    {
        SetWeaponType();
    }

    private void Start()
    {
        if (isPlayerWeapon)
        {
            firePoint = GameManager.Instance.GetWeaponsManager().GetCurrentFirePoint();
        }     
    }

    private void Update()
    {
        fireCooldown += Time.deltaTime;
    }

    public void AimAt(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public bool Shoot(Vector2 direction)
    {
        if (fireCooldown > fireRate)
        {
            if (isPlayerWeapon)
            {
                firePoint = GameManager.Instance.GetWeaponsManager().GetCurrentFirePoint();
            }

            fireCooldown = 0f;

            Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            newBullet.Activate(firePoint.position, direction, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable, isPlayerWeapon);

            if (isPlayerWeapon && weaponType != WeaponType.Pistol)
            {
                currentAmmo--;
            }
                
            return true;
        }

        return false;
    }

    private void SetWeaponType()
    {
        if (weaponType == WeaponType.Pistol)
        {
            fireRate = 0.25f;
            bulletSpeed = 15f;
            bulletLifeDistance = 18f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
            currentAmmo = 999999999;
        }
        else if (weaponType == WeaponType.Automatic)
        {
            fireRate = 0.16f;
            bulletSpeed = 25f;
            bulletLifeDistance = 18f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
            currentAmmo = 80;
        }
        else if (weaponType == WeaponType.Rifle)
        {
            fireRate = 0.35f;
            bulletSpeed = 35f;
            bulletLifeDistance = 30f;
            bulletDamage = 3;
            bulletIsDestroyable = false;
            currentAmmo = 40;
        }
    }

    public bool HasAmmo()
    {
        return currentAmmo > 0;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public Vector3 GetFirePointLocalPos()
    {
        return firePoint.localPosition;
    }

    public Vector3 GetFirePointWorldPos()
    {
        return firePoint.position;
    }

    public float GetWeaponRange()
    {
        return bulletLifeDistance;
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public void AddAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
    }
}
