using UnityEngine;

public class ToxicClud : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        transform.parent.GetComponent<ToxicFog>().OnTriggerStay2D(other);
    }
}
