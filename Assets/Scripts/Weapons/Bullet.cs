using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        MenuController.OnGameStarted += StopPause;
    }

    private void OnDestroy()
    {
        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
        MenuController.OnGameStarted -= StopPause;
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

        if (!collision.CompareTag("Enemy") && !collision.CompareTag("Player"))
        {
            Deactivate();
            Debug.Log("Bullet hit an obstacle and is deactivated.");
        }

        if (isPlayerBullet)
        {
            if (collision.CompareTag("Enemy"))
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
            else if (!collision.CompareTag("Player"))
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

    public void Activate(Vector2 startPosition, Vector2 direction, float speed, float lifeDistance, int damage, bool isDestroyable, bool isPlayerWeapon)
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

        isActive = true;
        gameObject.SetActive(true);
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
