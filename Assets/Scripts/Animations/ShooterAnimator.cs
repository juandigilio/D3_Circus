using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;


public class ShooterAnimator : MonoBehaviour
{
    [SerializeField] private Enemy_Shooter shooter;
    [SerializeField] private List<SpriteRenderer> walking = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> aiming_Front_Up = new List<SpriteRenderer>();
    [SerializeField] private Transform firePoint_Front_Up;
    [SerializeField] private List<SpriteRenderer> aiming_Front = new List<SpriteRenderer>();
    [SerializeField] private Transform firePoint_Front;
    [SerializeField] private List<SpriteRenderer> aiming_Front_Down = new List<SpriteRenderer>();
    [SerializeField] private Transform firePoint_Front_Down;
    [SerializeField] private List<SpriteRenderer> death = new List<SpriteRenderer>();
    [SerializeField] private float animationSpeed = 0.05f;


    private List<Transform> firePoints = new List<Transform>();
    private float animationTimer = 0f;
    private int walkingIndex = 0;
    private bool isShooting = false;
    private bool isDead = false;

    private void Start()
    {
        HideAll();

        firePoints.Clear();
        firePoints.Add(firePoint_Front_Up);
        firePoints.Add(firePoint_Front);
        firePoints.Add(firePoint_Front_Down);
    }

    private void Update()
    {
        if (shooter.IsPaused()) return;
        if (isDead) return;

        if (!isShooting && !isDead)
        {
            AnimateWalk();
        }
    }

    private void AnimateWalk()
    {
        animationTimer -= Time.deltaTime;

        if (animationTimer <= 0f)
        {
            animationTimer = animationSpeed;
            
            if (walkingIndex >= walking.Count)
                walkingIndex = 0;

            HideAll();

            walking[walkingIndex].enabled = true;
            walkingIndex++;
        }
    }

    public Vector3 AnimateShoot(int index)
    {
        StopAllCoroutines();

        switch (index)
        {
            case 0:
            {
                StartCoroutine(ShootCoroutine(aiming_Front_Up));
                return firePoint_Front_Up.position;
            }       
            case 1:
            {
                StartCoroutine(ShootCoroutine(aiming_Front));
                return firePoint_Front.position;
            }
            case 2:
            {
                StartCoroutine(ShootCoroutine(aiming_Front_Down));
                return firePoint_Front_Down.position;
            }
            default:
                return Vector3.zero;
        }
    }

    public void AnimateDeath()
    {
        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    public List<Transform> GetFirePoints()
    {
        return firePoints;
    }

    public bool IsShooting()
    {
        return isShooting;
    }

    private IEnumerator ShootCoroutine(List<SpriteRenderer> aimingSprites)
    {
        isShooting = true;

        foreach (var sprite in aimingSprites)
        {
            HideAll();
            sprite.enabled = true;
            yield return new WaitForSeconds(animationSpeed * 0.75f);
        }

        //HideAll();
        animationTimer = 0;
        isShooting = false;
    }

    private IEnumerator DeathCoroutine()
    {
        isDead = true;

        for (int i = 0; i < death.Count; i++)
        {
            HideAll();
            death[i].enabled = true;

            yield return new WaitForSeconds(animationSpeed);
        }

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    private void HideAll()
    {
        foreach (var sprite in walking)
            sprite.enabled = false;

        foreach (var sprite in aiming_Front_Up)
            sprite.enabled = false;

        foreach (var sprite in aiming_Front)
            sprite.enabled = false;

        foreach (var sprite in aiming_Front_Down)
            sprite.enabled = false;

        foreach (var sprite in death)
            sprite.enabled = false;
    }
}
