using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIAudio uiAudio;


    private InputManager inputManager;
    private PlayerController playerController;
    private Camera mainCamera;
    private SideScrollCamera SideScrollCamera;
    private LevelManager levelManager;
    private WeaponsManager weaponsManager;
    private JumperEnemiesManager jumperEnemiesManager;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterInputManager(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }

    public void RegisterPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void UnregisterPlayerController()
    {
        playerController = null;

        inputManager.UnregisterPlayerController();
    }

    public void RegisterMainCamera(Camera camera)
    {
        this.mainCamera = camera;
    }

    public void RegisterSideSrollCamera(SideScrollCamera sideScrollCamera)
    {
        this.SideScrollCamera = sideScrollCamera;
    }

    public void RegisterLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    public void RegisterWeaponsManager(WeaponsManager weaponsManager)
    {
        this.weaponsManager = weaponsManager;
    }

    public void RegisterJumperEnemiesManager(JumperEnemiesManager jumperEnemiesManager)
    {
        this.jumperEnemiesManager = jumperEnemiesManager;
    }

    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    public InputManager GetInputManager()
    {
        return inputManager;
    }

    public PlayerController GetPlayerController()
    {
        return playerController;
    }

    public Camera GetMainCamera()
    {
        return mainCamera;
    }

    public SideScrollCamera GetSideScrollCamera()
    { 
        return SideScrollCamera;
    }

    public UIAudio GetUIAudio()
    {
        return uiAudio;
    }

    public LevelManager GetLevelManager()
    {
        return levelManager;
    }

    public WeaponsManager GetWeaponsManager()
    {
        return weaponsManager;
    }

    public JumperEnemiesManager GetJumperEnemiesManager()
    {
        return jumperEnemiesManager;
    }
}
