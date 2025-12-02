using UnityEngine;

public enum WeaponType
{
    Pistol,
    Automatic,
    ShotGun,
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

    public bool Shoot(Vector2 direction, float angle)
    {
        if (fireCooldown > fireRate)
        {
            if (isPlayerWeapon)
            {
                firePoint = GameManager.Instance.GetWeaponsManager().GetCurrentFirePoint();
            }

            fireCooldown = 0f;

            if (isPlayerWeapon && weaponType == WeaponType.ShotGun)
            {
                float spread = 10f;

                CreateBullet(firePoint.position, direction, angle);

                CreateBullet(
                    firePoint.position,
                    RotateVector(direction, spread),
                    angle + spread
                );

                CreateBullet(
                    firePoint.position,
                    RotateVector(direction, -spread),
                    angle - spread
                );

                currentAmmo--;
                return true;
            }

            Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            newBullet.Activate(firePoint.position, direction, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable, isPlayerWeapon, weaponType, angle);

            if (isPlayerWeapon && weaponType != WeaponType.Pistol)
            {
                currentAmmo--;
            }

            return true;
        }

        return false;
    }

    public bool Shoot(Vector2 direction, Vector2 newPoint, float angle)
    {
        if (fireCooldown > fireRate)
        {
            fireCooldown = 0f;

            Bullet newBullet = Instantiate(bulletPrefab, newPoint, Quaternion.identity);
            newBullet.Activate(newPoint, direction, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable, isPlayerWeapon, weaponType, angle);

            if (isPlayerWeapon && weaponType != WeaponType.Pistol)
            {
                currentAmmo--;
            }

            return true;
        }
        return false;
    }

    private void CreateBullet(Vector2 pos, Vector2 dir, float angle)
    {
        Bullet b = Instantiate(bulletPrefab, pos, Quaternion.identity);
        b.Activate(pos, dir, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable, isPlayerWeapon, weaponType, angle);
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
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
        else if (weaponType == WeaponType.ShotGun)
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
