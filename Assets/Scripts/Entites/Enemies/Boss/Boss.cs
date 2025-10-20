using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Mouth")]
    [SerializeField] private GameObject mouth;
    [SerializeField] private Transform mouthStart;
    [SerializeField] private Transform mouthEnd;
    [SerializeField] private float mouthSpeed = 1.5f;

    [Header("Cannon")]
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private FireBall fireBallPrefab;
    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private float fireballSpeed = 2f;

    [Header("Attak")]
    [SerializeField] private float idleTime = 4f;
    [SerializeField] private float attackDuration = 3f;

    private Coroutine mouthRoutine;
    private Coroutine shootRoutine;


    protected override void Start()
    {
        base.Start();
        Debug.Log("Boss started");
        mouth.transform.position = mouthStart.position;

        health = 15;

        Attack();
    }

    protected void Update()
    {
        Patroll();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private IEnumerator MoveMouth(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * mouthSpeed;
            mouth.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    private void ShootFireball(Transform cannon, bool lefCannon)
    {
        Vector3 target;

        if (lefCannon)
        {
            target = targets[Random.Range(0, targets.Count / 2)].position;
        }
        else 
        {
            target = targets[Random.Range(targets.Count / 2, targets.Count)].position;
        }

        FireBall fb = Instantiate(fireBallPrefab, cannon.position, Quaternion.identity);

        fb.Init(cannon.position, target, arcHeight, fireballSpeed);
    }

    private IEnumerator ShootPattern()
    {
        while (true)
        {
            if (!isPaused)
            {
                ShootFireball(leftCannon, true);
                ShootFireball(rightCannon, false);
                yield return new WaitForSeconds(fireRate);
            }   
        }
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);

            isAttacking = true;

            yield return StartCoroutine(MoveMouth(mouthStart.position, mouthEnd.position));

            shootRoutine = StartCoroutine(ShootPattern());

            yield return new WaitForSeconds(attackDuration);

            StopCoroutine(shootRoutine);

            yield return StartCoroutine(MoveMouth(mouthEnd.position, mouthStart.position));

            isAttacking = false;
        }
    }

    protected override void Attack()
    {
        StartCoroutine(AttackCycle());
    }
}
