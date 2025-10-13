using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;


    private InputManager inputManager;
    private PlayerController playerController;
    private Camera mainCamera;
    private SideScrollCamera SideScrollCamera;


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

    public void RegisterMainCamera(Camera camera)
    {
        this.mainCamera = camera;
    }

    public void RegisterSideSrollCamera(SideScrollCamera sideScrollCamera)
    {
        this.SideScrollCamera = sideScrollCamera;
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
}
