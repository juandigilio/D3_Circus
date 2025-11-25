using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float levelTime = 300f;
    [SerializeField] private JumperEnemiesManager jumperEnemiesManager;
    [SerializeField] private GameObject shootersParent;

    private GameData gameData;
    private bool isPaused = false;


    private void Start()
    {
        GameManager.Instance.RegisterLevelManager(this);

        gameData.killsScore = 0;
        gameData.enemiesKilled = 0;
        gameData.collectablesScore = 0;
        gameData.collectedItems = 0;
        gameData.timeBonus = 0f;
        gameData.totalTime = levelTime;

        GameManager.Instance.GetMusicController().SetLevelState();

        PlayerController.OnPlayerDied += LoadGameOver;
        Boss.OnBossDied += LoadWin;

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDied -= LoadGameOver;
        Boss.OnBossDied -= LoadWin;

        PauseHandler.OnGameContinue -= StopPause;
        PauseHandler.OnGamePaused -= SetPaused;
    }

    private void FixedUpdate()
    {
        if (isPaused) return;

        if (gameData.totalTime > 0f)
        {
            gameData.totalTime -= Time.fixedDeltaTime;

            if (gameData.totalTime <= 0f)
            {
                gameData.totalTime = 0f;
                //LoadGameOver();
            }
        }
    }

    public void NotifyJumperEnemiesCleared()
    {
       GameManager.Instance.GetSideScrollCamera().Unlock();
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

    public float GetLevelTime()
    {
        return levelTime;
    }

    public float GetTotalTime()
    {
        return gameData.totalTime;
    }

    public int GetCurrentScore()
    {
        return gameData.killsScore + gameData.collectablesScore;
    }

    private void SetPaused()
    {
        isPaused = true;
    }

    private void StopPause()
    {
        isPaused = false;
    }

    private void LoadGameOver()
    {
        gameData.gameOver = true;
        CalculateScore();
        PlayerInfo.SetEndGame(gameData);
        LoadEndGame();

        GameManager.Instance.GetMusicController().SetDeathState();
    }

    private void LoadWin()
    {
        gameData.gameOver = false;
        CalculateScore();
        PlayerInfo.SetEndGame(gameData);
        LoadEndGame();

        GameManager.Instance.GetMusicController().SetCreditsState();
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

    public void KillAll()
    {
        foreach (Enemy_Shooter shooter in shootersParent.GetComponentsInChildren<Enemy_Shooter>())
        {
            shooter.TakeDamage(9999);
        }

        jumperEnemiesManager.KillAllJumpers();
    }
}
