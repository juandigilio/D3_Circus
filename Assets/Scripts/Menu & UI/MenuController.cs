using UnityEngine;


public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuButtonsObj;
    [SerializeField] private GameObject backButtonObj;
    [SerializeField] private GameObject creditsObj;
    [SerializeField] private GameObject optionsObj;

    private static GameObject menuButtons;
    private static GameObject backButton;
    private static GameObject credits;
    private static GameObject options;

    public static event System.Action OnGameStarted;

    private void Start()
    {
        menuButtons = menuButtonsObj;
        backButton = backButtonObj;
        credits = creditsObj;
        options = optionsObj;

        ShowMenu();
    }

    private void Update()
    {
        CheckInput();
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

    public static void ShowMenu()
    {
        options.SetActive(false);
        credits.SetActive(false);
        backButton.SetActive(false);
        menuButtons.SetActive(true);
    }

    public async void LoadGame()
    {
        OnGameStarted?.Invoke();

        await SceneManager.LoadGameAsync();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu();
            Debug.Log("Showing Menu from MenuController");
        }
    }

    public void CloseGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
