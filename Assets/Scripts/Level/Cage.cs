using UnityEngine;
using System.Collections;

public class Cage : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer_1;
    [SerializeField] private SpriteRenderer spriteRenderer_2;
    [SerializeField] private int health = 8;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int flashCount = 2;

    private Color originalColor_1;
    private Color originalColor_2;
    private bool isFlashing = false;

    private void Start()
    {
        if (spriteRenderer_1 != null) originalColor_1 = spriteRenderer_1.color;
        if (spriteRenderer_2 != null) originalColor_2 = spriteRenderer_2.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        if (isFlashing) yield break;
        isFlashing = true;

        for (int i = 0; i < flashCount; i++)
        {
            SetColor(Color.red);
            yield return new WaitForSeconds(flashDuration);
            ResetColor();
            yield return new WaitForSeconds(flashDuration);
        }

        isFlashing = false;
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer_1 != null) spriteRenderer_1.color = color;
        if (spriteRenderer_2 != null) spriteRenderer_2.color = color;
    }

    private void ResetColor()
    {
        if (spriteRenderer_1 != null) spriteRenderer_1.color = originalColor_1;
        if (spriteRenderer_2 != null) spriteRenderer_2.color = originalColor_2;
    }
}
