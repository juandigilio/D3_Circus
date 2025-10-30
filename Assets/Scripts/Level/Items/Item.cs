using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;

    protected virtual void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUp();
            Destroy(gameObject);
        }
    }

    protected abstract void PickUp();
}
