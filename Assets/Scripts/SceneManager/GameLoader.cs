using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private CustomScene gameLoader;
    [SerializeField] private CustomScene mainScene;
    [SerializeField] private CustomScene mainMenu;
    [SerializeField] private List<CustomScene> scenesPool;
    [SerializeField] private CustomScene winingScene;
    [SerializeField] private CustomScene gameOverScene;


    private async void Start()
    {
        SceneManager.SetScenes(gameLoader, mainScene, mainMenu, scenesPool, winingScene, gameOverScene);

        await SceneManager.LoadMenuSceneAsync();
    }
}
