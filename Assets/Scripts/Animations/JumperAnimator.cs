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

    private void Start()
    {
        HideAll();
        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (jumper.IsPaused()) return;
       
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
        StartCoroutine(AnimateAttak());
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

    private IEnumerator AnimateAttak()
    {
        for (int i = 0; i < attaking.Count; i++)
        {
            HideAll();
            attaking[i].enabled = true;

            yield return new WaitForSeconds(animationSpeed);
        }

        jumper.RetreatJump();
        SetWalking(true);
    }

    public void AnimateDeath()
    {
        StopAllCoroutines();
        
        isJumping = false;
        isWalking = false;
        isAttaking = false;


        StopAllCoroutines();
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        Debug.Log("Animating Death");

        for (int i = 0; i < death.Count; i++)
        {
            HideAll();
            death[i].enabled = true;

            yield return new WaitForSeconds(animationSpeed);
        }
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
