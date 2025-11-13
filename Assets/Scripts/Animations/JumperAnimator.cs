using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAnimator : MonoBehaviour
{
    [SerializeField] private Enemy_Jumper jumper;
    [SerializeField] private List<SpriteRenderer> walking = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> attaking = new List<SpriteRenderer>();
    [SerializeField] private float animationSpeed = 0.05f;

    private float animationTimer = 0f;
    private int walkingIndex = 0;
    private int attakingIndex = 0;
    private int jumpingIndex = 0;
    private bool isWalking = false;
    private bool isAttaking = false;
    private bool isJumping = false;

    private void Start()
    {
        HideAll();
        isWalking = true;
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
    }

    public void SetAttaking(bool attacking)
    {
        if (isAttaking == attacking) return;

        isAttaking = attacking;
        isWalking = !attacking;
        isJumping = !attacking;

        attakingIndex = 0;

        StopAllCoroutines();
        StartCoroutine(AnimateAttak());

        Debug.Log("Animating Attack" + isAttaking);
    }

    public void SetIsJumping(bool jumping)
    {
        if (isJumping == jumping) return;

        isJumping = jumping;
        isWalking = !jumping;
        isAttaking = !jumping;

        jumpingIndex = 0;
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

    private IEnumerator AnimateAttak()
    {
        Debug.Log("Animating Attack");
        for (int i = 0; i < attaking.Count; i++)
        {
            HideAll();
            attaking[i].enabled = true;

            yield return new WaitForSeconds(animationSpeed);
        }

        jumper.RetreatJump();
        SetWalking(true);
    }

    private void AnimateJump()
    {
        animationTimer += Time.fixedDeltaTime;

        if (animationTimer >= animationSpeed)
        {
            HideAll();
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
    }
}
