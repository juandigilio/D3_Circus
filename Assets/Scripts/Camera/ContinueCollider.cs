using UnityEngine;

public class ContinueCollider : MonoBehaviour
{
    [SerializeField] private SideScrollCamera sideScrollCamera;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sideScrollCamera.Continue();
            gameObject.SetActive(false);
        }
    }
}
