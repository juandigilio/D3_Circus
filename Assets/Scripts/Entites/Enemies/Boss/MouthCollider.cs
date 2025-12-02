using UnityEngine;

public class MouthCollider : MonoBehaviour
{
    [SerializeField] public Collider2D mouthCollider;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
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
        //Destroy(rb);
    }
}
