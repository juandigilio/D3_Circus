using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    public static event System.Action OnGamePaused;
    public static event System.Action OnGameContinue;

    private bool isPaused = false;


    private void Start()
    {
        optionsMenu.SetActive(false);
    }

    private void Update()
    {
        HandlePause();
    }

    

    private void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                SetPause();
            }
            else
            {
                StopPause();
            }
        }
    }

    private void SetPause()
    {
        OnGamePaused?.Invoke();
        OpenOptionsMenu();
        isPaused = true;
    }

    public void StopPause()
    {
        OnGameContinue?.Invoke();
        CloseOptionsMenu();
        isPaused = false;
    }

    public async void BackToMenu()
    {
        await SceneManager.GoBackToMenuAsync();
    }

    private void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    private void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);
    }
}
