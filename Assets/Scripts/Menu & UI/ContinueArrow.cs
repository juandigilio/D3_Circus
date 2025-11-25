using UnityEngine;

public class ContinueArrow : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.RegisterContinueArrow(this.gameObject);
        gameObject.SetActive(false);
    }
}
