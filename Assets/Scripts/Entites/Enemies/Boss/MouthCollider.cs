using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    [SerializeField] private Collider2D mouthCollider;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.tag == "Ground")
        {
            mouthCollider.isTrigger = false;
        }
    }

    public void TurnOffRigidbody()
    {
        //rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(rb);
    }
}
