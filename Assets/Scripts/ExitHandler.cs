using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    public static event System.Action OnGamePaused;


    private void Update()
    {
        HandleExit();
    }

    private void HandleExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.IsMainMenuSceneLoaded())
            {
                //MenuController.ShowMenu();
            }
            else
            {
                //OnGamePaused?.Invoke();
                SceneManager.TogglePause();
            }      
        }
    }
}
