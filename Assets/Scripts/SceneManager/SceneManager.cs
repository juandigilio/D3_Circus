using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager
{
    //public static event Action OnGameStarted;

    private static HashSet<string> loadedScenes = new HashSet<string>();
    private static CustomScene gameLoaderScene;
    private static CustomScene mainScene;
    private static CustomScene mainMenuScene;
    private static List<CustomScene> scenesPool = new List<CustomScene>();
    private static CustomScene winingScene;
    private static CustomScene gameOverScene;

    private static int index = 0;



    private static async Task LoadSceneAsync(CustomScene scene)
    {
        if (scene.sceneName == null || string.IsNullOrEmpty(scene.sceneName))
        {
            Debug.LogWarning("LoadSceneAsync called with null scene.");
            return;
        }

        if (!IsSceneLoaded(scene))
        {
            Debug.Log($"Loading scene: {scene.sceneName}");

            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
                await Task.Yield();

            loadedScenes.Add(scene.sceneName);
        }
    }

    private static async Task UnloadSceneAsync(CustomScene scene)
    {
        if (scene.sceneName == null || string.IsNullOrEmpty(scene.sceneName))
        {
            Debug.LogWarning("UnloadSceneAsync called with null scene.");
            return;
        }

        if (IsSceneLoaded(scene))
        {
            Debug.Log($"Unloading scene: {scene.sceneName}");

            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.sceneName);
            while (!asyncUnload.isDone)
                await Task.Yield();

            loadedScenes.Remove(scene.sceneName);
        }
    }

    private static bool IsSceneLoaded(CustomScene scene) =>
        loadedScenes.Contains(scene.sceneName);

    private static async Task LoadMainScene()
    {
        await LoadSceneAsync(mainScene);
        await UnloadSceneAsync(gameLoaderScene);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(mainScene.sceneName));
    }

    public static void SetScenes(CustomScene gameLoader, CustomScene main, CustomScene menu, List<CustomScene> sceneDictionary, CustomScene win, CustomScene gameOver)
    {
        gameLoaderScene = gameLoader;
        mainScene = main;
        mainMenuScene = menu;
        winingScene = win;
        gameOverScene = gameOver;

        scenesPool.Clear();
        scenesPool.AddRange(sceneDictionary);

        loadedScenes.Clear();
        if (gameLoaderScene.sceneName != null)
            loadedScenes.Add(gameLoaderScene.sceneName);
    }

    public static async Task LoadNextSceneAsync()
    {
        if ((index + 1) < scenesPool.Count)
        {
            index++;
            await LoadSceneAsync(scenesPool[index]);
        }
    }

    public static async Task UnloadLastSceneAsync()
    {
        if (index > 0)
        {
            await UnloadSceneAsync(scenesPool[index - 1]);
        }
    }

    public static async Task LoadGameAsync()
    {
        await LoadTutorialSceneAsync();    
        await UnloadSceneAsync(mainMenuScene);
        await UnloadSceneAsync(mainMenuScene);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(scenesPool[0].sceneName));
    }

    public static async Task LoadMenuSceneAsync()
    {
        await LoadMainScene();
        await LoadSceneAsync(mainMenuScene);
        await UnloadSceneAsync(gameLoaderScene);

        foreach (CustomScene scene in scenesPool)
            await UnloadSceneAsync(scene);
    }

    public static async Task LoadTutorialSceneAsync()
    {
        index = 0;
        await LoadSceneAsync(scenesPool[index]);
    }

    public static async Task UnloadMainMenuSceneAsync()
    {
        await UnloadSceneAsync(mainMenuScene);
    }

    public static async Task LoadWiningSceneAsync()
    {
        await LoadSceneAsync(winingScene);
    }

    public static bool IsMainMenuSceneLoaded() =>
        IsSceneLoaded(mainMenuScene);


    public static async Task GoBackToMenuAsync()
    {
        await LoadMenuSceneAsync();
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(mainScene.sceneName));
    }
}
