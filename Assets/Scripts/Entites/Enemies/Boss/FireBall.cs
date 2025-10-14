using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private float height;
    private float duration;
    private float timer;

    public void Init(Vector3 start, Vector3 target, float arcHeight, float speed)
    {
        startPos = start;
        targetPos = target;
        height = arcHeight;
        duration = speed;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime / duration;

        float t = Mathf.Clamp01(timer);

        Vector3 horizontal = Vector3.Lerp(startPos, targetPos, t);

        float arc = Mathf.Sin(t * Mathf.PI) * height;

        transform.position = new Vector3(horizontal.x, horizontal.y + arc, horizontal.z);

        if (t >= 1f)
            Destroy(gameObject);
    }
}
