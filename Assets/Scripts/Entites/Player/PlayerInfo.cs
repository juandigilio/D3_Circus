using UnityEngine;

public enum InputType
{
    Separated,
    Combined,
    Mouse
}

public struct GameData
{
    public bool gameOver;
    public int killsScore;
    public int enemiesKilled;
    public int collectablesScore;
    public int collectedItems;
    public float totalTime;
    public float timeBonus;
    public int totalScore;
    public int maxScore;
}

public static class PlayerInfo
{
    private static InputType inputType = InputType.Combined;

    private static GameData gameData = new GameData();

    private const string MAX_SCORE_KEY = "MaxScore";
    private const string INPUT_TYPE_KEY = "InputType";


    public static void LoadPlayerData()
    {
        gameData.maxScore = PlayerPrefs.GetInt(MAX_SCORE_KEY, 0);
        inputType = (InputType)PlayerPrefs.GetInt(INPUT_TYPE_KEY, (int)InputType.Combined);
    }

    public static void SavePlayerData()
    {
        if (gameData.totalScore > gameData.maxScore)
            gameData.maxScore = gameData.totalScore;

        PlayerPrefs.SetInt(MAX_SCORE_KEY, gameData.maxScore);
        PlayerPrefs.SetInt(INPUT_TYPE_KEY, (int)inputType);
        PlayerPrefs.Save();
    }

    public static void SetInputType(InputType input)
    {
        inputType = input;

        PlayerPrefs.SetInt(INPUT_TYPE_KEY, (int)inputType);
        PlayerPrefs.Save();
    }

    public static InputType GetInputType()
    {
        return inputType;
    }

    public static void SetEndGame(GameData newData)
    {
        gameData.gameOver = newData.gameOver;
        gameData.killsScore = newData.killsScore;
        gameData.enemiesKilled = newData.enemiesKilled;
        gameData.collectablesScore = newData.collectablesScore;
        gameData.collectedItems = newData.collectedItems;
        gameData.totalTime = newData.totalTime;
        gameData.timeBonus = newData.timeBonus;
        gameData.totalScore = newData.totalScore;
    }

    public static GameData GetGameData() => gameData;
}
