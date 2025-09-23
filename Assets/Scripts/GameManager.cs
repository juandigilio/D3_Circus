using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private InputManager inputManager;
    private PlayerController playerController;
    private Camera mainCamera;


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
}
