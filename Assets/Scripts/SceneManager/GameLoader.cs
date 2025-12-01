using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private CustomScene gameLoader;
    [SerializeField] private CustomScene mainScene;
    [SerializeField] private CustomScene mainMenu;
    [SerializeField] private CustomScene cutScene;
    [SerializeField] private List<CustomScene> scenesPool;
    [SerializeField] private CustomScene endScene;
    [SerializeField] private CustomScene creditsScene;


    private async void Start()
    {
        SceneManager.SetScenes(gameLoader, mainScene, mainMenu, cutScene, scenesPool, endScene, creditsScene);

        await SceneManager.LoadMenuSceneAsync();
    }
}
