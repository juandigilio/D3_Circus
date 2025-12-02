using UnityEngine;
using UnityEngine.EventSystems;


public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuButtonsObj;
    [SerializeField] private GameObject backButtonObj;
    [SerializeField] private GameObject creditsObj;
    [SerializeField] private GameObject optionsObj;
    [SerializeField] private CreditsManager creditsManager;

    private static GameObject menuButtons;
    private static GameObject backButton;
    private static GameObject credits;
    private static GameObject options;

    private static bool gameLoaded = false;

    

    private void Start()
    {
        menuButtons = menuButtonsObj;
        backButton = backButtonObj;
        credits = creditsObj;
        options = optionsObj;

        PlayerInfo.LoadPlayerData();

        ShowMenu();

        GameManager.Instance.GetMusicController().SetGameStart();
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

        var first = backButton;
        if (first != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }

    public void ShowCredits()
    {
        menuButtons.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
        backButton.SetActive(false);

        creditsManager.BeginCredits();

        var first = options.GetComponentInChildren<UnityEngine.UI.Button>();
        if (first != null)
            EventSystem.current.SetSelectedGameObject(first.gameObject);
    }

    public static void ShowMenu()
    {
        options.SetActive(false);
        credits.SetActive(false);
        backButton.SetActive(false);
        menuButtons.SetActive(true);

        var firstButton = menuButtons.GetComponentInChildren<UnityEngine.UI.Button>();
        if (firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);

        gameLoaded = false;
    }

    public async void LoadGame()
    {
        if (gameLoaded) return;

        gameLoaded = true;

        await SceneManager.LoadCutSceneAsync();
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
