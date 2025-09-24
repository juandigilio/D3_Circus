using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;

    protected virtual void Start()
    {
        playerController = GameManager.Instance.GetPlayerController();
        //if (playerController == null)
        //{
        //    Debug.LogError("PlayerController not found in GameManager.");
        //}
        //else
        //{
        //    Debug.Log("PlayerController successfully referenced in Item.");
        //}
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
