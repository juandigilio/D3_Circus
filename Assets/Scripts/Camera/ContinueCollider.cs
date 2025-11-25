using UnityEngine;

public class ContinueCollider : MonoBehaviour
{
    [SerializeField] private SideScrollCamera sideScrollCamera;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sideScrollCamera.Continue();
            GameManager.Instance.GetContinueArrow().SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
