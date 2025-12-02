using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float levelTime = 300f;
    [SerializeField] private JumperEnemiesManager jumperEnemiesManager;
    [SerializeField] private List<Sector> shooterSectors = new List<Sector>();

    private GameData gameData;
    private bool isPaused = false;
    private int currentSector = 0;


    private void Start()
    {
        GameManager.Instance.RegisterLevelManager(this);

        gameData.killsScore = 0;
        gameData.enemiesKilled = 0;
        gameData.collectablesScore = 0;
        gameData.collectedItems = 0;
        gameData.timeBonus = 0f;
        gameData.totalTime = levelTime;

        currentSector = 0;

        //HideAll();
        shooterSectors[currentSector].gameObject.SetActive(true);

        GameManager.Instance.GetMusicController().SetLevelState();

        PlayerController.OnPlayerDied += LoadGameOver;
        Boss.OnEndGame += LoadWin;
        Boss.OnBossDied += SetPaused;

        PauseHandler.OnGameContinue += StopPause;
        PauseHandler.OnGamePaused += SetPaused;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDied -= LoadGameOver;
        Boss.OnEndGame -= LoadWin;
        Boss.OnBossDied -= SetPaused;

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
                LoadGameOver();
            }

            CheckActiveEnemies();
        }  
    }

    public void LoadNextSector()
    {
        jumperEnemiesManager.LoadNextSector();

        shooterSectors[currentSector].gameObject.SetActive(false);  

        if (currentSector + 1 >= shooterSectors.Count) return;

        currentSector++;
        shooterSectors[currentSector].gameObject.SetActive(true);
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

    private void CheckActiveEnemies()
    {
        if (GameManager.Instance.GetSideScrollCamera().IsMoving()) return;

        if (jumperEnemiesManager.IsCleared())
        {
            if (!shooterSectors[currentSector].IsSectorCleared()) return;

            foreach (Cage cage in shooterSectors[currentSector].GetCages())
            {
                cage.TurnOffCoins();
            }

            GameManager.Instance.GetSideScrollCamera().Unlock();
        }
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
        jumperEnemiesManager.gameObject.SetActive(false);
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

    public void HideAll()
    {
        foreach (Sector sector in shooterSectors)
        {
            sector.gameObject.SetActive(false);
        }
    }
}
