using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SceneManager
{
    private static HashSet<string> loadedScenes = new HashSet<string>();
    private static CustomScene gameLoaderScene;
    private static CustomScene mainScene;
    private static CustomScene mainMenuScene;
    private static List<CustomScene> scenesPool = new List<CustomScene>();
    private static CustomScene winingScene;

    private static int index = 0;


    private static void LoadScene(CustomScene scene)
    {
        if (!IsSceneLoaded(scene))
        {
            loadedScenes.Add(scene.sceneName);

            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.sceneName);
        }

        SetInputActionMap(scene);
    }

    private static async Task UnloadScene(CustomScene scene)
    {
        if (IsSceneLoaded(scene))
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.sceneName);

            while (!asyncUnload.isDone)
            {
                await Task.Yield();
            }
            loadedScenes.Remove(scene.sceneName);
        }
    }

    private static bool IsSceneLoaded(CustomScene scene)
    {
        return loadedScenes.Contains(scene.sceneName);
    }

    public static void LoadMainScene()
    {
        LoadScene(mainScene);
        LoadFirstLevel();
    }

    private static async Task UnloadAll()
    {
        await UnloadScene(gameLoaderScene);

        foreach (CustomScene scene in scenesPool)
        {
            await UnloadScene(scene);
        }
        
        await UnloadScene(winingScene);
    }

    private static void SetInputActionMap(CustomScene scene)
    {
        InputManager inputManager = GameManager.Instance.GetInputManager();

        //inputManager.SetActionMap(scene.actionMapType);
    }

    public static void SetScenes(CustomScene gameLoader, CustomScene main, CustomScene menu, List<CustomScene> sceneDictionary, CustomScene win, CustomScene gameOver)
    {
        gameLoaderScene = gameLoader;
        mainScene = main;
        mainMenuScene = menu;

        foreach (CustomScene scene in sceneDictionary)
        {
            scenesPool.Add(scene);
        }

        winingScene = win;
    }

    public static void LoadNextScene()
    {
        if ((index + 1) < scenesPool.Count)
        {
            index++;
            LoadScene(scenesPool[index]);
        }
    }

    public static void UnloadLastScene()
    {
        if (index > 0)
        {
            _ = UnloadScene(scenesPool[index - 1]);
        }
    }

    public static void LoadGame()
    {
        LoadMenuScene();
        _ = UnloadScene(gameLoaderScene);
    }

    public static void LoadMenuScene()
    {
        _ = UnloadAll();
        LoadScene(mainMenuScene);
    }

    public static void LoadFirstLevel()
    {
        UnloadMainMenuScene();

        index = 0;
        LoadScene(scenesPool[index]);
    }

    public static void UnloadMainMenuScene()
    {
        _ = UnloadScene(mainMenuScene);
    }

    public static void LoadWiningScene()
    {
        LoadScene(winingScene);
    }

    public static bool IsMainMenuSceneLoaded()
    {
        return IsSceneLoaded(mainMenuScene);
    }
}
