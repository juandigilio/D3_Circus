using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadMainScene();
    } 

    public void ShowInstructions()
    {

    }   

    public void ShowOptions()
    {

    }

    public void ShowCredits()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
