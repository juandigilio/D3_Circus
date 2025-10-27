using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float levelTime = 300f;

    private int totalScore = 0;
    private int enemiesKilled = 0;
    private float totalTime = 0f;



    private void Start()
    {
        PlayerController.OnPlayerDied += LoadGameOver;
        Boss.OnBossDied += LoadWin;
    }

    private void Update()
    {
        
    }

    private void LoadGameOver()
    {
        PlayerInfo.SetEndGame(true, totalScore, totalTime, enemiesKilled);
        LoadEndGame();
    }

    private void LoadWin()
    {
        PlayerInfo.SetEndGame(false, totalScore, totalTime, enemiesKilled);
        LoadEndGame();
    }

    private async void LoadEndGame()
    {
        await SceneManager.LoadEndSceneAsync();
    }
}
