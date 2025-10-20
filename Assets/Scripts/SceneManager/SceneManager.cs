using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager
{
    public static event Action OnGameStarted;
    public static event Action OnGamePaused;

    private static HashSet<string> loadedScenes = new HashSet<string>();
    private static CustomScene gameLoaderScene;
    private static CustomScene mainScene;
    private static CustomScene mainMenuScene;
    private static List<CustomScene> scenesPool = new List<CustomScene>();
    private static CustomScene pauseScene;
    private static CustomScene winingScene;

    private static int index = 0;


    private static void LoadSceneAsync(CustomScene scene)
    {
        if (!IsSceneLoaded(scene))
        {
            loadedScenes.Add(scene.sceneName);

            //AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.sceneName, LoadSceneMode.Additive);

            //while (!asyncLoad.isDone)
            //{
            //    await Task.Yield();
            //}
        }
    }

    private static void UnloadSceneAsync(CustomScene scene)
    {
        if (IsSceneLoaded(scene))
        {
            UnityEngine.SceneManagement.SceneManager.UnloadScene(scene.sceneName);
            //AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.sceneName);

            Debug.Log("Unloading scene: " + scene.sceneName);

            //while (!asyncUnload.isDone)
            //{
            //    await Task.Yield();
            //}

            loadedScenes.Remove(scene.sceneName);
        }
    }

    private static bool IsSceneLoaded(CustomScene scene)
    {
        return loadedScenes.Contains(scene.sceneName);
    }

    private static void LoadMainScene()
    {
        LoadSceneAsync(mainScene);
    }

    //private static async Task UnloadAll()
    //{
    //    await UnloadSceneAsync(gameLoaderScene);

    //    foreach (CustomScene scene in scenesPool)
    //    {
    //        await UnloadSceneAsync(scene);
    //    }

    //    await UnloadSceneAsync(winingScene);
    //}

    //private static void SetInputActionMap(CustomScene scene)
    //{
    //    //InputManager inputManager = GameManager.Instance.GetInputManager();

    //    //inputManager.SetActionMap(scene.actionMapType);
    //}

    public static void SetScenes(CustomScene gameLoader, CustomScene main, CustomScene menu, List<CustomScene> sceneDictionary, CustomScene pause, CustomScene win, CustomScene gameOver)
    {
        gameLoaderScene = gameLoader;
        mainScene = main;
        mainMenuScene = menu;

        foreach (CustomScene scene in sceneDictionary)
        {
            scenesPool.Add(scene);
        }

        pauseScene = pause;
        winingScene = win;

        loadedScenes.Add(gameLoaderScene.sceneName);
    }

    public static void LoadNextSceneAsync()
    {
        if ((index + 1) < scenesPool.Count)
        {
            index++;

            LoadSceneAsync(scenesPool[index]);
        }
    }

    public static void UnloadLastScene()
    {
        if (index > 0)
        {
            UnloadSceneAsync(scenesPool[index - 1]);
        }
    }

    public static void LoadGame()
    {
        UnloadSceneAsync(mainMenuScene);

        LoadTutorialScene();
    }

    private static void LoadMenu()
    {
        LoadMainScene();

        UnloadSceneAsync(gameLoaderScene);

        LoadSceneAsync(mainMenuScene);
    }

    public static void LoadMenuScene()
    {
        LoadMenu();

        UnloadSceneAsync(gameLoaderScene);

        foreach (CustomScene scene in scenesPool)
        {
            UnloadSceneAsync(scene);
        }
    }

    public static void LoadTutorialScene()
    {
        index = 0;

        LoadSceneAsync(scenesPool[index]);
    }

    public static void UnloadMainMenuScene()
    {
        UnloadSceneAsync(mainMenuScene);
    }

    public static void LoadWiningScene()
    {
        LoadSceneAsync(winingScene);
    }

    public static bool IsMainMenuSceneLoaded()
    {
        return IsSceneLoaded(mainMenuScene);
    }

    private static void LoadPause()
    {
        LoadSceneAsync(pauseScene);
    }

    private static void UnloadPause()
    {
        UnloadSceneAsync(pauseScene);
    }

    public static void TogglePause()
    {
        if (IsSceneLoaded(pauseScene))
        {
            OnGameStarted?.Invoke();
            UnloadPause();
        }
        else
        {
            OnGamePaused?.Invoke();
            LoadPause();
        }
    }

    public static void GobackToMenu()
    {
        LoadMenuScene();
    }
}
