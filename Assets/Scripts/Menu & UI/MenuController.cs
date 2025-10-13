using UnityEngine;


public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject options;

    private void Start()
    {
        ShowMenu();
    }

    public void ShowOptions()
    {
        menuButtons.SetActive(false);
        credits.SetActive(false);
        options.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowCredits()
    {
        menuButtons.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
        backButton.SetActive(true);
    }

    public void ShowMenu()
    {
        options.SetActive(false);
        credits.SetActive(false);
        backButton.SetActive(false);
        menuButtons.SetActive(true);
    }

    public void LoadGame()
    {
        _ = SceneManager.LoadGame();
    }

    public void CloseGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
