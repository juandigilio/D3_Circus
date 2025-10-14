using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : Enemy
{
    [SerializeField] private GameObject mouth;
    [SerializeField] private Transform mouthStart;
    [SerializeField] private Transform mouthEnd;
    [SerializeField] private float mouthSpeed = 1.5f;
    [SerializeField] private float openDuration = 2f;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;
    [SerializeField] private FireBall fireBallPrefab;
    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private float fireballSpeed = 2f;


    private Coroutine mouthRoutine;
    private Coroutine shootingRoutine;
    private bool isMouthOpen = false;


    protected override void Start()
    {
        base.Start();
        Debug.Log("Boss started");
        mouth.transform.position = mouthStart.position;
    }

    protected void Update()
    {
        Patroll();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMouthOpen)
                StartOpenMouth();
        }

        Attack();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void StartOpenMouth()
    {
        if (mouthRoutine != null)
            StopCoroutine(mouthRoutine);

        mouthRoutine = StartCoroutine(OpenMouthRoutine());
    }

    private IEnumerator OpenMouthRoutine()
    {
        isMouthOpen = true;

        yield return MoveMouth(mouthStart.position, mouthEnd.position);

        yield return new WaitForSeconds(openDuration);

        yield return MoveMouth(mouthEnd.position, mouthStart.position);

        isMouthOpen = false;
        mouthRoutine = null;
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
            ShootFireball(leftCannon, true);
            ShootFireball(rightCannon, false);
            yield return new WaitForSeconds(fireRate);
        }
    }

    protected override void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shootingRoutine == null)
                shootingRoutine = StartCoroutine(ShootPattern());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (shootingRoutine != null)
            {
                StopCoroutine(shootingRoutine);
                shootingRoutine = null;
            }
        }
    }
}
