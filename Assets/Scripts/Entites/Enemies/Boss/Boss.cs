using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Mouth")]
    [SerializeField] private MouthCollider mouthCollider;
    [SerializeField] private Collider2D mouthTrigger;
    [SerializeField] private GameObject mouth;
    [SerializeField] private Transform mouthStart;
    [SerializeField] private Transform mouthEnd;
    [SerializeField] private float mouthSpeed = 1.5f;
    [SerializeField] private SpriteRenderer mouthRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

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

    [Header("Audio")]
    [SerializeField] private BossAudio bossAudio;

    private Coroutine mouthRoutine;
    private Coroutine shootRoutine;
    private Color originalMouthColor;
    private Coroutine flashRoutine;


    public static event System.Action OnBossDied;


    protected override void Start()
    {
        base.Start();
        mouth.transform.position = mouthStart.position;
        mouthTrigger.enabled = false;

        originalMouthColor = mouthRenderer.color;

        GameManager.Instance.RegisterBoss(this);

        Attack();
    }

    protected void Update()
    {
        if (isPaused) return;
        if (playerController.CurrentHealth() <= 0) return;
        Patroll();
    }

    protected override void FixedUpdate()
    {
        if (playerController.CurrentHealth() <= 0) return;
        base.FixedUpdate();
    }

    protected override void UpdateAssetDirection()
    {
    }

    private IEnumerator MoveMouth(Vector3 from, Vector3 to)
    {
        float t = 0f;

        while (t < 1f)
        {
            while (isPaused)
                yield return null;

            t += Time.deltaTime * mouthSpeed;
            Vector3 pos = Vector3.Lerp(from, to, t);
            mouth.transform.position = pos;
            mouthTrigger.transform.position = pos;

            yield return null;
        }
    }

    private IEnumerator FlashMouth()
    {
        mouthRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        mouthRenderer.color = originalMouthColor;
    }

    private void ShootFireball(Transform cannon, bool lefCannon)
    {
        if (playerController.CurrentHealth() <= 0) return;

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
            while (isPaused)
                yield return null;

            ShootFireball(leftCannon, true);
            ShootFireball(rightCannon, false);

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);

            bossAudio.PlayLaughSound();
            isAttacking = true;
            mouthTrigger.enabled = true;

            yield return StartCoroutine(MoveMouth(mouthStart.position, mouthEnd.position));

            shootRoutine = StartCoroutine(ShootPattern());

            yield return new WaitForSeconds(attackDuration);

            StopCoroutine(shootRoutine);

            yield return StartCoroutine(MoveMouth(mouthEnd.position, mouthStart.position));
            mouthTrigger.enabled = false;

            isAttacking = false;
        }
    }

    private IEnumerator FallMouth()
    {
        isPaused = true;
        isAttacking = false;

        mouthCollider.TurnOffRigidbody();
        //if (mouthCollider != null)
        //    mouthCollider.isTrigger = false;

        Rigidbody2D rb = mouth.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = mouth.AddComponent<Rigidbody2D>();

        rb.gravityScale = 2f;
        rb.freezeRotation = false;
        rb.angularVelocity = Random.Range(-250f, 250f);

        GameManager.Instance.GetLevelManager().KillAll();

        yield return new WaitForSeconds(2f);

        OnBossDied?.Invoke();
    }

    protected override void Attack()
    {
        StartCoroutine(AttackCycle());
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashMouth());

        if (health <= 0)
        {
            StartCoroutine(FallMouth());
        }

        //enemyAudio.PlayHitSound();
    }

    public BossAudio GetBossAudio()
    {
        return bossAudio;
    }
}
