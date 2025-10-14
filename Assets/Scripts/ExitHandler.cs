using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ExitHandler : MonoBehaviour
{
    public static event System.Action OnGamePaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnGamePaused?.Invoke();
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        _ = SceneManager.LoadMenu();
    }
}
