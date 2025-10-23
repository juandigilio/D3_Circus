using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private GameObject dev;
    [SerializeField] private GameObject art;
    [SerializeField] private GameObject sound;
    [SerializeField] private GameObject qa;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private async void BackToMenu()
    {
        await SceneManager.GoBackToMenuAsync();
    }

    private void ShowCredits()
    {

    }

    private void SetPositions()
    {

    }
}
