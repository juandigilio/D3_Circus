using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.U2D;

public class Cage : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> lockSprites = new List<SpriteRenderer>();
    [SerializeField] private Collider2D lockCollider;
    [SerializeField] private GameObject colliderLeft;
    [SerializeField] private GameObject colliderRight;
    [SerializeField] private int maxHealth = 16;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 2;

    private List<Color> originalColors = new List<Color>();
    private bool isFlashing = false;
    private int currentHealth = 0;
    private Rigidbody2D rb;

    private void Start()
    {
        originalColors.Clear();

        foreach (SpriteRenderer sprite in lockSprites)
        {
            Color color = sprite.color;
            originalColors.Add(color);
            sprite.enabled = false;
        }

        lockSprites[0].enabled = true;

        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < maxHealth * 0.30f)
        {
            HideAll();
            lockSprites[2].enabled = true;
        }
        else if (currentHealth < maxHealth * 0.7f)
        {
            HideAll();
            lockSprites[1].enabled = true;
        }
        
        foreach (SpriteRenderer sprite in lockSprites)
        {
            if (sprite.enabled)
            {
                StartCoroutine(FlashRed(sprite));
                break;
            }
        }

        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyCoroutine());
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        HideAll();
        lockSprites[3].enabled = true;
        ResetColor();

        Destroy(colliderLeft);
        Destroy(colliderRight);

        rb.constraints = RigidbodyConstraints2D.None;

        lockCollider.isTrigger = false;

        for (int i = 0; i < flashCount * 3; i++)
        {
            SetColor(lockSprites[3], new Color(1, 1, 1, 0));
            yield return new WaitForSeconds(flashDuration);
            ResetColor();
            yield return new WaitForSeconds(flashDuration);
        }

        Destroy(gameObject);
    }

    private IEnumerator FlashRed(SpriteRenderer sprite)
    {
        if (isFlashing) yield break;
        isFlashing = true;

        for (int i = 0; i < flashCount; i++)
        {
            SetColor(sprite, Color.red);
            yield return new WaitForSeconds(flashDuration);
            ResetColor();
            yield return new WaitForSeconds(flashDuration);
        }

        isFlashing = false;
    }

    private void HideAll()
    {
        foreach (SpriteRenderer sprite in lockSprites)
        {
            sprite.enabled = false;
        }
    }

    private void SetColor(SpriteRenderer sprite, Color color)
    {
        sprite.color = color;
    }

    private void ResetColor()
    {
        int index = 0;

        foreach (SpriteRenderer sprite in lockSprites)
        {
            sprite.color = originalColors[index];
            index++;
        }
    }
}
