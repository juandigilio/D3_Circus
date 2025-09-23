using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected PlayerController playerController;

    protected virtual void Start()
    {
        playerController = GameManager.Instance.GetComponent<PlayerController>();

        Debug.Log("Entered ");
        Debug.Assert(playerController != null, "PlayerController not found in the scene. Make sure there is a GameManager with a PlayerController.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUp();
            Destroy(gameObject);
        }
    }

    protected abstract void PickUp();
}
