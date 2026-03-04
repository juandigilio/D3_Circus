using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAnimator : MonoBehaviour
{
    [SerializeField] private Enemy_Jumper jumper;
    [SerializeField] private List<SpriteRenderer> walking = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> attaking = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> jumping = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> death = new List<SpriteRenderer>();
    [SerializeField] private float animationSpeed = 0.05f;

    private float animationTimer = 0f;
    private int walkingIndex = 0;
    private int jumpingIndex = 0;
    private bool isWalking = false;
    private bool isAttaking = false;
    private bool isJumping = false;
    private bool isDeath = false;
    private float deathTimer = 0f;

    private void Start()
    {
        HideAll();
        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (isDeath)
        {
            return;
        }

        if (jumper.IsPaused() || jumper.IsDead()) return;
       
        UpdateAnimations();
    }

    public void SetWalking(bool walking)
    {
        if (isWalking == walking) return;

        isWalking = walking;
        isAttaking = !walking;
        isJumping = !walking;

        walkingIndex = 0;
        animationTimer = animationSpeed;
    }

    public void SetAttacking(bool attacking)
    {
        if (isAttaking == attacking) return;

        isAttaking = attacking;
        isWalking = !attacking;
        isJumping = !attacking;

        StopAllCoroutines();
        StartCoroutine(AnimateAttack());
    }

    public void SetIsJumping()
    {
        isJumping = true;
        isWalking = false;
        isAttaking = false;

        jumpingIndex = 0;
        animationTimer = animationSpeed * 2;
    }

    public void SetFalling()
    {
        isJumping = true;
        isWalking = false;
        isAttaking = false;

        jumpingIndex = 1;
    }

    private void UpdateAnimations()
    {
        if (isWalking)
        {
            AnimateWalk();
        }
        else if(isJumping)
        {
            AnimateJump();
        }
    }

    private void AnimateWalk()
    {
        animationTimer += Time.fixedDeltaTime;

        if (animationTimer >= animationSpeed)
        {
            HideAll();
            animationTimer = 0f;

            walking[walkingIndex].enabled = true;

            walkingIndex++;

            if (walkingIndex >= walking.Count)
            {
                walkingIndex = 0;
            }
        }
    }

    private void AnimateJump()
    {
        if (jumpingIndex >= jumping.Count) return;

        animationTimer += Time.fixedDeltaTime;

        if (animationTimer >= animationSpeed * 2)
        {
            HideAll();

            animationTimer = 0f;

            jumping[jumpingIndex].enabled = true;
            jumpingIndex++;
        }
    }

    private IEnumerator AnimateAttack()
    {
        for (int i = 0; i < attaking.Count; i++)
        {
            if (jumper.IsDead()) yield break;

            HideAll();
            attaking[i].enabled = true;

            yield return new WaitForSeconds(animationSpeed);
        }

        if (jumper.IsDead()) yield break;

        jumper.RetreatJump();
        SetIsJumping();
    }

    public void AnimateDeath()
    {
        if (isDeath)
            return;
        
        isJumping = false;
        isWalking = false;
        isAttaking = false;
        isDeath = true;

        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        for (int i = 0; i < death.Count; i++)
        {
            HideAll();
            death[i].enabled = true;

            yield return new WaitForSeconds(0.14f);
        }

        yield return new WaitForSeconds(0.3f);

        float blinkTime = 0.1f;
        int blinkCount = 8;

        for (int i = 0; i < blinkCount; i++)
        {
            SetDeathSpritesVisible(false);
            yield return new WaitForSeconds(blinkTime * 1.6f);

            SetDeathSpritesVisible(true);
            yield return new WaitForSeconds(blinkTime);

            blinkTime -= 0.01f;
        }

        Destroy(gameObject);
    }

    private void SetDeathSpritesVisible(bool visible)
    {
        death[1].enabled = visible;
    }

    private void HideAll()
    {
        foreach (SpriteRenderer sprite in walking)
        {
            sprite.enabled = false;
        }
        foreach (SpriteRenderer sprite in attaking)
        {
            sprite.enabled = false;
        }
        foreach (SpriteRenderer sprite in jumping)
        {
            sprite.enabled = false;
        }
        foreach (SpriteRenderer sprite in death)
        {
            sprite.enabled = false;
        }
    }
}
