using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float levelTime = 300f;

    private GameData gameData;



    private void Start()
    {
        GameManager.Instance.RegisterLevelManager(this);

        gameData.killsScore = 0;
        gameData.enemiesKilled = 0;
        gameData.collectablesScore = 0;
        gameData.collectedItems = 0;
        gameData.timeBonus = 0f;
        gameData.totalTime = levelTime;

        PlayerController.OnPlayerDied += LoadGameOver;
        Boss.OnBossDied += LoadWin;
    }

    private void FixedUpdate()
    {
        if (gameData.totalTime > 0f)
        {
            gameData.totalTime -= Time.fixedDeltaTime;

            if (gameData.totalTime <= 0f)
            {
                gameData.totalTime = 0f;
                LoadGameOver();
            }
        }
    }

    public void AddKillScore(int value)
    {
        gameData.killsScore += value;
        gameData.enemiesKilled++;
    }

    public void AddItemScore(int value)
    {
        gameData.collectablesScore += value;
        gameData.collectedItems++;
    }

    public float GetTotalTime()
    {
        return gameData.totalTime;
    }

    public int GetCurrentScore()
    {
        return gameData.killsScore + gameData.collectablesScore;
    }

    private void LoadGameOver()
    {
        gameData.gameOver = true;
        CalculateScore();
        PlayerInfo.SetEndGame(gameData);
        LoadEndGame();
    }

    private void LoadWin()
    {
        gameData.gameOver = false;
        CalculateScore();
        PlayerInfo.SetEndGame(gameData);
        LoadEndGame();
    }

    private void CalculateScore()
    {
        gameData.timeBonus = (100 / levelTime) * gameData.totalTime;
        gameData.totalScore = gameData.killsScore + gameData.collectablesScore;
        gameData.totalScore += Mathf.RoundToInt((gameData.timeBonus / 100) * gameData.totalScore);
    }

    private async void LoadEndGame()
    {
        await SceneManager.LoadEndSceneAsync();
    }
}
