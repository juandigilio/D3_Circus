using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float shakeAmount = 0.005f;
    [SerializeField] float shakeSpeed = 40f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float height;
    private float duration;
    private float timer;
    private int damage = 1;

    

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = GameManager.Instance.GetPlayerController();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    private void Move()
    {
        timer += Time.deltaTime / duration;

        float t = Mathf.Clamp01(timer);

        Vector3 horizontal = Vector3.Lerp(startPos, targetPos, t);
        float arc = Mathf.Sin(t * Mathf.PI) * height;
        Vector3 basePos = new Vector3(horizontal.x, horizontal.y + arc, horizontal.z);

        basePos = Shake(basePos);

        transform.position = basePos;

        if (t >= 1f)
            Destroy(gameObject);
    }

    private Vector3 Shake(Vector3 basePos)
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed + GetInstanceID()) * shakeAmount;
        float offsetY = Mathf.Cos(Time.time * shakeSpeed * 1.3f + GetInstanceID()) * shakeAmount;

        basePos += new Vector3(offsetX, offsetY, 0);

        return basePos;
    }

    public void Init(Vector3 start, Vector3 target, float arcHeight, float speed)
    {
        startPos = start;
        targetPos = target;
        height = arcHeight;
        duration = speed;
        timer = 0f;
    }
}
