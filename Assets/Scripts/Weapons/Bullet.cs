using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pistolBullet;
    [SerializeField] private GameObject pistolAnimator;
    [SerializeField] private SpriteRenderer machineGunBullet;
    [SerializeField] private GameObject machineGunAnimator;
    [SerializeField] private SpriteRenderer shotGunBullet;
    [SerializeField] private GameObject shotGunAnimator;

    private PlayerController player;
    private Vector2 startPosition;
    private Vector2 direction;

    private float speed;
    private float lifeDistance;
    private int damage;
    private bool isDestroyable;
    private bool isActive = false;
    private bool isPlayerBullet = true;
    private bool isPaused = false;

    private void Start()
    {
        player = GameManager.Instance.GetPlayerController();

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
        CutSceneManager.OnGameStarted += StopPause;
    }

    private void OnDestroy()
    {
        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
        CutSceneManager.OnGameStarted -= StopPause;
    }

    private void Update()
    {
        if (!isActive) return;

        if (!isPaused)
        {
            transform.Translate(direction * speed * Time.deltaTime);

            if (Vector2.Distance(startPosition, transform.position) >= lifeDistance)
            {
                Deactivate();
            }
            else
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);

                if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
                {
                    Deactivate();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if (isPaused) return;

        if (collision.CompareTag("Baloon"))
        {
            Deactivate();

            Baloon baloon = collision.GetComponent<Baloon>();
            if (baloon != null)
            {
                GameManager.Instance.GetUIAudio().PlayBallonPopSound();
                baloon.Pop();
            }
        }

        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Player") &&
            !collision.CompareTag("Cloud") && !collision.CompareTag("Item") &&
            !collision.CompareTag("Boss") && !collision.CompareTag("Bullet") &&
            !collision.CompareTag("Fireball"))
        {
            Deactivate();
        }

        if (isPlayerBullet)
        {
            if (collision.CompareTag("Cage"))
            {
                Deactivate();

                Cage cage = collision.GetComponent<Cage>();

                if (cage != null)
                {
                    cage.TakeDamage(damage);
                }
            }
            else if (collision.CompareTag("Boss"))
            {
                Deactivate();

                GameManager.Instance.GetBoss().TakeDamage(damage);
            }
            else if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);

                    if (isDestroyable)
                    {
                        Deactivate();
                    }
                }
                else
                {
                    Debug.LogError("Enemy component missing on the collided object.");
                }
            }

            if (!collision.CompareTag("Player") && !collision.CompareTag("Cloud") &&
                !collision.CompareTag("Fireball") && !collision.CompareTag("Item") &&
                !collision.CompareTag("Bullet"))
            {
                Deactivate();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.TakeDamage(damage);

                    if (isDestroyable)
                    {
                        Deactivate();
                    }
                }
            }
        }
    }

    public void Activate(Vector2 startPosition, Vector2 direction, float speed, float lifeDistance, int damage, bool isDestroyable, bool isPlayerWeapon, WeaponType type, float angle)
    {
        transform.position = startPosition;
        this.startPosition = startPosition;
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifeDistance = lifeDistance;
        this.damage = damage;
        this.isDestroyable = isDestroyable;
        this.isPlayerBullet = isPlayerWeapon;

        if (!isPlayerWeapon)
        {
            damage = 1;
        }

        gameObject.SetActive(true);

        switch (type)
        {
            case WeaponType.Pistol:
            {
                pistolBullet.enabled = true;
                machineGunBullet.enabled = false;
                shotGunBullet.enabled = false;
                pistolBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                pistolAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);
                machineGunAnimator.SetActive(false);
                shotGunAnimator.SetActive(false);
                pistolAnimator.SetActive(true);
                    break;
            }
            case WeaponType.Automatic:
            {
                pistolBullet.enabled = false;
                machineGunBullet.enabled = true;                 
                shotGunBullet.enabled = false;
                machineGunBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                machineGunAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);
                pistolAnimator.SetActive(false);
                pistolAnimator.isStatic = true;
                machineGunAnimator.SetActive(true);
                shotGunAnimator.SetActive(false);
                    break;
            }
            case WeaponType.ShotGun:
            {
                pistolBullet.enabled = false;
                machineGunBullet.enabled = false;
                shotGunBullet.enabled = true;
                shotGunBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                shotGunAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);
                pistolAnimator.SetActive(false);
                machineGunAnimator.SetActive(false);
                shotGunAnimator.SetActive(true);
                break;
            }
            default:
            {
                pistolBullet.enabled = true;
                machineGunBullet.enabled = false;
                shotGunBullet.enabled = false;
                pistolBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                pistolAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);
                pistolAnimator.SetActive(true);
                machineGunAnimator.SetActive(false);
                shotGunAnimator.SetActive(false);
                    break;
            }
        }

        isActive = true;
    }

    public bool IsPlayerBullet()
    {
        return isPlayerBullet;
    }

    public int GetDamage()
    {
        return damage;
    }

    private void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void SetPaused()
    {
        isPaused = true;
    }

    private void StopPause()
    {
        isPaused = false;
    }
}
